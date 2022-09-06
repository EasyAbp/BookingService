using System;
using System.Threading.Tasks;
using Orleans;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleChangedEventHandler :
    ILocalEventHandler<EntityCreatedEventData<AssetSchedule>>,
    ILocalEventHandler<EntityUpdatedEventData<AssetSchedule>>,
    ILocalEventHandler<EntityDeletedEventData<AssetSchedule>>,
    ITransientDependency
{
    private readonly IGrainFactory _grainFactory;
    private readonly ICurrentTenant _currentTenant;

    public AssetScheduleChangedEventHandler(IGrainFactory grainFactory,
        ICurrentTenant currentTenant)
    {
        _grainFactory = grainFactory;
        _currentTenant = currentTenant;
    }

    public virtual Task HandleEventAsync(EntityCreatedEventData<AssetSchedule> eventData)
    {
        var grain = _grainFactory.GetGrain<IAssetScheduleGrain>(eventData.Entity.AssetId,
            CalculateCompoundKey(eventData.Entity.Date));
        return grain.AddOrUpdateAsync(ToStateModel(eventData.Entity));
    }

    public virtual Task HandleEventAsync(EntityUpdatedEventData<AssetSchedule> eventData)
    {
        var grain = _grainFactory.GetGrain<IAssetScheduleGrain>(eventData.Entity.AssetId,
            CalculateCompoundKey(eventData.Entity.Date));
        return grain.AddOrUpdateAsync(ToStateModel(eventData.Entity));
    }

    public virtual Task HandleEventAsync(EntityDeletedEventData<AssetSchedule> eventData)
    {
        var grain = _grainFactory.GetGrain<IAssetScheduleGrain>(eventData.Entity.AssetId,
            CalculateCompoundKey(eventData.Entity.Date));
        return grain.DeleteAsync(ToStateModel(eventData.Entity));
    }

    protected virtual string CalculateCompoundKey(DateTime date)
    {
        return AssetScheduleExtensions.CalculateCompoundKey(date, _currentTenant.Id);
    }

    protected virtual AssetScheduleGrainStateModel ToStateModel(AssetSchedule entity)
    {
        return entity.ToAssetScheduleGrainStateModel();
    }
}