using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public interface IAssetOccupyTransactionManager
{
    Task<IAsyncDisposable> TryAcquireAsync(IEnumerable<Guid> assetCategoryIds, IOccupyingTimeInfo occupyingTimeInfo,
        TimeSpan timeout = default);
}

public class DefaultAssetOccupyTransactionManager : IAssetOccupyTransactionManager, ITransientDependency
{
    protected IDistributedCache<AssetOccupyTransactionCacheItem> DistributedCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IClock Clock { get; }

    public DefaultAssetOccupyTransactionManager(
        IDistributedCache<AssetOccupyTransactionCacheItem> distributedCache,
        IAbpDistributedLock distributedLock,
        IGuidGenerator guidGenerator,
        IClock clock)
    {
        DistributedCache = distributedCache;
        DistributedLock = distributedLock;
        GuidGenerator = guidGenerator;
        Clock = clock;
    }

    public virtual async Task<IAsyncDisposable> TryAcquireAsync(IEnumerable<Guid> assetCategoryIds,
        IOccupyingTimeInfo occupyingTimeInfo,
        TimeSpan timeout = default)
    {
        /*
         * we use the wait-die method here to prevent "all transaction fail" when serious competition.
         * more info: http://www.mathcs.emory.edu/~cheung/Courses/554/Syllabus/8-recv+serial/deadlock-waitdie.html
         */
        var transactionId = GuidGenerator.Create();
        var timestamp = Clock.Now.Ticks;
        var cacheItem = new AssetOccupyTransactionCacheItem(transactionId, timestamp);
        var cts = new CancellationTokenSource(timeout);
        var keys = new List<string>();
        var handles = new List<IAbpDistributedLockHandle>();

        foreach (var assetCategoryId in assetCategoryIds)
        {
            var key = AssetOccupyTransactionCacheItem.CalculateKey(assetCategoryId, occupyingTimeInfo);
            var handle = await DistributedLock.TryAcquireAsync(key, TimeSpan.Zero, cts.Token);
            if (handle is null)
            {
                var item = await GetCacheItemAsync(key, token: cts.Token);
                if (item is null)
                {
                    // TODO use new Exception?
                    throw new FailToObtainAssetOccupancyLockException(assetCategoryId, occupyingTimeInfo);
                }

                if (item.Timestamp > timestamp)
                {
                    // current transaction is older, wait for until the lock is released (by the younger transaction)
                    handle = await DistributedLock.TryAcquireAsync(key, timeout, cts.Token);
                    if (handle is null)
                    {
                        // TODO use TimoutToObtainAssetOccupancyLockException ?
                        throw new FailToObtainAssetOccupancyLockException(assetCategoryId, occupyingTimeInfo);
                    }

                    await DistributedCache.SetAsync(key, cacheItem, token: cts.Token);
                }
                else if (item.Timestamp < timestamp)
                {
                    // current transaction is younger, abort
                    throw new FailToObtainAssetOccupancyLockException(assetCategoryId, occupyingTimeInfo);
                }
                else
                {
                    if (item.TransactionId.CompareTo(transactionId) > 0)
                    {
                        // current transaction is older, wait for until the lock is released (by the younger transaction)
                        handle = await DistributedLock.TryAcquireAsync(key, timeout, cts.Token);
                        if (handle is null)
                        {
                            // TODO use TimoutToObtainAssetOccupancyLockException ?
                            throw new FailToObtainAssetOccupancyLockException(assetCategoryId, occupyingTimeInfo);
                        }

                        await DistributedCache.SetAsync(key, cacheItem, token: cts.Token);
                    }
                    else
                    {
                        // current transaction is younger
                        throw new FailToObtainAssetOccupancyLockException(assetCategoryId, occupyingTimeInfo);
                    }
                }
            }
            else
            {
                await DistributedCache.SetAsync(key, cacheItem, token: cts.Token);
            }

            keys.Add(key);
            handles.Add(handle);
        }

        return new AsyncDisposeFunc(async () =>
        {
            // ReSharper disable once MethodSupportsCancellation
            await DistributedCache.RemoveManyAsync(keys, hideErrors: true);
            foreach (var handle in handles)
            {
                try
                {
                    await handle.DisposeAsync();
                }
                catch
                {
                    // ignored
                }
            }
        });
    }

    protected virtual async Task<AssetOccupyTransactionCacheItem> GetCacheItemAsync(string key, CancellationToken token)
    {
        var item = await DistributedCache.GetAsync(key, token: token);
        while (item is null && !token.IsCancellationRequested)
        {
            await Task.Delay(50, token);
            item = await DistributedCache.GetAsync(key, token: token);
        }

        token.ThrowIfCancellationRequested();
        return item;
    }
}

[Serializable]
public class AssetOccupyTransactionCacheItem
{
    public AssetOccupyTransactionCacheItem()
    {
    }

    public AssetOccupyTransactionCacheItem(Guid transactionId, long timestamp)
    {
        TransactionId = transactionId;
        Timestamp = timestamp;
    }

    public Guid TransactionId { get; set; }

    public long Timestamp { get; set; }

    public static string CalculateKey(Guid assetCategoryId, IOccupyingTimeInfo occupyingTimeInfo)
    {
        return $"{occupyingTimeInfo.Date:yyyyMMdd}|{assetCategoryId:N}";
    }
}