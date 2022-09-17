using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleStore : IAssetScheduleStore, ITransientDependency
{
    protected IDistributedCache<AssetScheduleCacheItem> DistributedCache { get; }
    protected IAssetScheduleRepository AssetScheduleRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public AssetScheduleStore(IDistributedCache<AssetScheduleCacheItem> distributedCache,
        IAssetScheduleRepository assetScheduleRepository,
        ICurrentTenant currentTenant)
    {
        DistributedCache = distributedCache;
        AssetScheduleRepository = assetScheduleRepository;
        CurrentTenant = currentTenant;
    }

    public virtual async Task<List<AssetSchedule>> GetListAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        CancellationToken token = default)
    {
        var item = await DistributedCache.GetOrAddAsync(CalculateKey(date, assetId, periodSchemeId),
            () => AssetScheduleCacheItemFactory(date, assetId, periodSchemeId), token: token);
        return item.Items;
    }

    protected virtual string CalculateKey(DateTime date, Guid assetId, Guid periodSchemeId)
    {
        return AssetScheduleCacheItem.CalculateKey(date, assetId, periodSchemeId, CurrentTenant.Id);
    }

    protected virtual async Task<AssetScheduleCacheItem> AssetScheduleCacheItemFactory(DateTime date, Guid assetId,
        Guid periodSchemeId)
    {
        var list = await AssetScheduleRepository.GetListAsync(date, assetId, periodSchemeId);
        return new AssetScheduleCacheItem(list);
    }
}