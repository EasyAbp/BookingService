using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleCacheInvalidator : ILocalEventHandler<EntityChangedEventData<AssetSchedule>>,
    ITransientDependency
{
    protected IDistributedCache<AssetScheduleCacheItem> DistributedCache { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public AssetScheduleCacheInvalidator(IDistributedCache<AssetScheduleCacheItem> distributedCache,
        ICurrentTenant currentTenant)
    {
        DistributedCache = distributedCache;
        CurrentTenant = currentTenant;
    }

    public virtual Task HandleEventAsync(EntityChangedEventData<AssetSchedule> eventData)
    {
        var key = AssetScheduleCacheItem.CalculateKey(eventData.Entity.Date,
            eventData.Entity.AssetId,
            eventData.Entity.PeriodSchemeId,
            CurrentTenant.Id);
        return DistributedCache.RemoveAsync(key);
    }
}