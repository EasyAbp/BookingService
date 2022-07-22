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

public class DefaultAssetOccupyTransactionLock : IAssetOccupyTransactionLock, ITransientDependency
{
    protected IDistributedCache<AssetOccupyTransactionCacheItem> DistributedCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IClock Clock { get; }

    public DefaultAssetOccupyTransactionLock(
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

    public virtual async Task<IAsyncDisposable> TryAcquireAsync(
        IEnumerable<(Guid CategoryId, IOccupyingTimeInfo Model)> resources,
        TimeSpan timeout)
    {
        /*
         * we use the wait-die method here to prevent "all transaction fail" when serious competition.
         * more info: http://www.mathcs.emory.edu/~cheung/Courses/554/Syllabus/8-recv+serial/deadlock-waitdie.html
         */
        var transactionId = GuidGenerator.Create();
        var timestamp = Clock.Now.Ticks;
        var cacheItem = new AssetOccupyTransactionCacheItem(transactionId, timestamp);
        var keys = new HashSet<string>();
        var handles = new List<IAbpDistributedLockHandle>();

        foreach (var resource in resources)
        {
            var key = AssetOccupyTransactionCacheItem.CalculateKey(resource.CategoryId, resource.Model);
            if (!keys.Add(key))
            {
                continue;
            }

            var handle = await DistributedLock.TryAcquireAsync(key, TimeSpan.Zero);
            if (handle is null)
            {
                var item = await GetCacheItemAsync(key, timeout);
                if (item is null)
                {
                    throw new FailToObtainAssetOccupancyLockException(resource.CategoryId, resource.Model);
                }

                if (item.Timestamp > timestamp)
                {
                    // current transaction is older, wait for until the lock is released (by the younger transaction)
                    handle = await DistributedLock.TryAcquireAsync(key, timeout);
                    if (handle is null)
                    {
                        throw new FailToObtainAssetOccupancyLockException(resource.CategoryId, resource.Model);
                    }

                    await DistributedCache.SetAsync(key, cacheItem);
                }
                else if (item.Timestamp < timestamp)
                {
                    // current transaction is younger, abort
                    throw new FailToObtainAssetOccupancyLockException(resource.CategoryId, resource.Model);
                }
                else
                {
                    if (item.TransactionId.CompareTo(transactionId) > 0)
                    {
                        // current transaction is older, wait for until the lock is released (by the younger transaction)
                        handle = await DistributedLock.TryAcquireAsync(key, timeout);
                        if (handle is null)
                        {
                            throw new FailToObtainAssetOccupancyLockException(resource.CategoryId, resource.Model);
                        }

                        await DistributedCache.SetAsync(key, cacheItem);
                    }
                    else
                    {
                        // current transaction is younger
                        throw new FailToObtainAssetOccupancyLockException(resource.CategoryId, resource.Model);
                    }
                }
            }
            else
            {
                await DistributedCache.SetAsync(key, cacheItem);
            }

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

    protected virtual async Task<AssetOccupyTransactionCacheItem> GetCacheItemAsync(string key, TimeSpan timeout)
    {
        var item = await DistributedCache.GetAsync(key);
        var cts = new CancellationTokenSource(timeout);
        while (item is null && !cts.IsCancellationRequested)
        {
            await Task.Delay(50, cts.Token);
            item = await DistributedCache.GetAsync(key, token: cts.Token);
        }

        cts.Token.ThrowIfCancellationRequested();
        return item;
    }
}