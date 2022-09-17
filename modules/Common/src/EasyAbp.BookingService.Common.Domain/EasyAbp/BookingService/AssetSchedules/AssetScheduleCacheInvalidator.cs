using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleCacheInvalidator : ILocalEventHandler<EntityChangedEventData<AssetSchedule>>,
    ITransientDependency
{
    protected IDistributedCache<AssetScheduleCacheItem> DistributedCache { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public AssetScheduleCacheInvalidator(IDistributedCache<AssetScheduleCacheItem> distributedCache,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager)
    {
        DistributedCache = distributedCache;
        CurrentTenant = currentTenant;
        UnitOfWorkManager = unitOfWorkManager;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<AssetSchedule> eventData)
    {
        var key = AssetScheduleCacheItem.CalculateKey(eventData.Entity.Date,
            eventData.Entity.AssetId,
            eventData.Entity.PeriodSchemeId,
            CurrentTenant.Id);
        await DistributedCache.RemoveAsync(key);
        UnitOfWorkManager.Current?.OnCompleted(() => DistributedCache.RemoveAsync(key));
    }
}