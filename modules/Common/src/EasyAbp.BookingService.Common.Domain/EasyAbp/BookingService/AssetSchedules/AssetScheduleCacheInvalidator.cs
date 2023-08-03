using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
    protected ICurrentTenant CurrentTenant { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    public AssetScheduleCacheInvalidator(
        IDistributedCache<AssetScheduleCacheItem> distributedCache,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceScopeFactory serviceScopeFactory)
    {
        DistributedCache = distributedCache;
        CurrentTenant = currentTenant;
        UnitOfWorkManager = unitOfWorkManager;
        ServiceScopeFactory = serviceScopeFactory;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<AssetSchedule> eventData)
    {
        var key = AssetScheduleCacheItem.CalculateKey(eventData.Entity.Date,
            eventData.Entity.AssetId,
            eventData.Entity.PeriodSchemeId,
            CurrentTenant.Id);

        await DistributedCache.RemoveAsync(key);

        UnitOfWorkManager.Current?.OnCompleted(async () =>
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache<AssetScheduleCacheItem>>();
            await cache.RemoveAsync(key);
        });
    }
}