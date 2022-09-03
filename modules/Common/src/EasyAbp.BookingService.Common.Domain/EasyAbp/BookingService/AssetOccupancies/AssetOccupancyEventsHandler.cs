using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancyProviders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyEventsHandler :
    IDistributedEventHandler<OccupyAssetEto>,
    IDistributedEventHandler<OccupyAssetByCategoryEto>,
    IDistributedEventHandler<BulkOccupyAssetEto>,
    ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IAssetOccupancyProvider _assetOccupancyProvider;

    public AssetOccupancyEventsHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IDistributedEventBus distributedEventBus,
        IAssetOccupancyProvider assetOccupancyProvider)
    {
        _currentTenant = currentTenant;
        _unitOfWorkManager = unitOfWorkManager;
        _distributedEventBus = distributedEventBus;
        _assetOccupancyProvider = assetOccupancyProvider;
    }

    public virtual async Task HandleEventAsync(OccupyAssetEto eventData)
    {
        using var changeTenant = _currentTenant.Change(eventData.TenantId);

        try
        {
            using var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions(isTransactional: true));

            var (_, occupancy) = await _assetOccupancyProvider.OccupyAsync(eventData.Model, eventData.OccupierUserId);

            await CreatePublishSuccessResultEventAsync(occupancy, eventData);

            await uow.CompleteAsync();
        }
        catch
        {
            await CreatePublishFailureResultEventAsync(eventData);
            throw;
        }
    }

    public virtual async Task HandleEventAsync(OccupyAssetByCategoryEto eventData)
    {
        using var changeTenant = _currentTenant.Change(eventData.TenantId);

        try
        {
            using var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions(isTransactional: true));

            var (_, occupancy) =
                await _assetOccupancyProvider.OccupyByCategoryAsync(eventData.Model, eventData.OccupierUserId);

            await CreatePublishSuccessResultEventAsync(occupancy, eventData);

            await uow.CompleteAsync();
        }
        catch
        {
            await CreatePublishFailureResultEventAsync(eventData);
            throw;
        }
    }

    public virtual async Task HandleEventAsync(BulkOccupyAssetEto eventData)
    {
        using var changeTenant = _currentTenant.Change(eventData.TenantId);

        try
        {
            using var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions(isTransactional: true));

            var result = await _assetOccupancyProvider.BulkOccupyAsync(eventData.Models, eventData.ByCategoryModels,
                eventData.OccupierUserId);

            var occupancies = result.Select(x => x.Item2);

            await CreatePublishSuccessResultEventAsync(occupancies, eventData);

            await uow.CompleteAsync();
        }
        catch
        {
            await CreatePublishFailureResultEventAsync(eventData);

            throw;
        }
    }

    protected virtual async Task CreatePublishSuccessResultEventAsync(AssetOccupancy occupancy, OccupyAssetEto inputEto)
    {
        var resultEto = new AssetOccupancyResultEto(occupancy.TenantId, inputEto.RequestId, true,
            MapToAssetOccupancyInfoModel(occupancy));

        inputEto.MapExtraPropertiesTo(resultEto, MappingPropertyDefinitionChecks.None);

        await _distributedEventBus.PublishAsync(resultEto);
    }

    protected virtual async Task CreatePublishSuccessResultEventAsync(AssetOccupancy occupancy,
        OccupyAssetByCategoryEto inputEto)
    {
        var resultEto = new AssetOccupancyResultEto(occupancy.TenantId, inputEto.RequestId, true,
            MapToAssetOccupancyInfoModel(occupancy));

        inputEto.MapExtraPropertiesTo(resultEto, MappingPropertyDefinitionChecks.None);

        await _distributedEventBus.PublishAsync(resultEto);
    }

    protected virtual async Task CreatePublishSuccessResultEventAsync(IEnumerable<AssetOccupancy> occupancies,
        BulkOccupyAssetEto inputEto)
    {
        var resultEto = new BulkAssetOccupancyResultEto(inputEto.TenantId, inputEto.RequestId, true,
            new List<AssetOccupancyInfoModel>());

        foreach (var occupancy in occupancies)
        {
            resultEto.Models.Add(MapToAssetOccupancyInfoModel(occupancy));
        }

        inputEto.MapExtraPropertiesTo(resultEto, MappingPropertyDefinitionChecks.None);

        await _distributedEventBus.PublishAsync(resultEto);
    }

    protected virtual async Task CreatePublishFailureResultEventAsync(OccupyAssetEto inputEto)
    {
        var resultEto = new AssetOccupancyResultEto(inputEto.TenantId, inputEto.RequestId, false, null);

        inputEto.MapExtraPropertiesTo(resultEto, MappingPropertyDefinitionChecks.None);

        await _distributedEventBus.PublishAsync(resultEto);
    }

    protected virtual async Task CreatePublishFailureResultEventAsync(OccupyAssetByCategoryEto inputEto)
    {
        var resultEto = new AssetOccupancyResultEto(inputEto.TenantId, inputEto.RequestId, false, null);

        inputEto.MapExtraPropertiesTo(resultEto, MappingPropertyDefinitionChecks.None);

        await _distributedEventBus.PublishAsync(resultEto);
    }

    protected virtual async Task CreatePublishFailureResultEventAsync(BulkOccupyAssetEto inputEto)
    {
        var resultEto = new AssetOccupancyResultEto(inputEto.TenantId, inputEto.RequestId, false, null);

        inputEto.MapExtraPropertiesTo(resultEto, MappingPropertyDefinitionChecks.None);

        await _distributedEventBus.PublishAsync(resultEto);
    }

    protected virtual AssetOccupancyInfoModel MapToAssetOccupancyInfoModel(AssetOccupancy assetOccupancy)
    {
        return new AssetOccupancyInfoModel(assetOccupancy.Id, assetOccupancy.AssetId, assetOccupancy.Asset,
            assetOccupancy.AssetDefinitionName, assetOccupancy.Volume, assetOccupancy.Date, assetOccupancy.StartingTime,
            assetOccupancy.Duration, assetOccupancy.OccupierUserId, assetOccupancy.OccupierName);
    }
}