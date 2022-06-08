using System.Collections.Generic;
using System.Threading.Tasks;
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
    private readonly AssetOccupancyManager _assetOccupancyManager;
    private readonly IAssetOccupancyRepository _assetOccupancyRepository;

    public AssetOccupancyEventsHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IDistributedEventBus distributedEventBus,
        AssetOccupancyManager assetOccupancyManager,
        IAssetOccupancyRepository assetOccupancyRepository)
    {
        _currentTenant = currentTenant;
        _unitOfWorkManager = unitOfWorkManager;
        _distributedEventBus = distributedEventBus;
        _assetOccupancyManager = assetOccupancyManager;
        _assetOccupancyRepository = assetOccupancyRepository;
    }

    public virtual async Task HandleEventAsync(OccupyAssetEto eventData)
    {
        using var changeTenant = _currentTenant.Change(eventData.TenantId);

        try
        {
            using var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions(isTransactional: true));

            var occupancy = await _assetOccupancyManager.CreateAsync(eventData.Model, eventData.OccupierUserId);

            await _assetOccupancyRepository.InsertAsync(occupancy, true);

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

            var occupancy =
                await _assetOccupancyManager.CreateByCategoryIdAsync(eventData.Model, eventData.OccupierUserId);

            await _assetOccupancyRepository.InsertAsync(occupancy, true);

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
            var occupancies = new List<AssetOccupancy>();
            using var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions(isTransactional: true));

            foreach (var model in eventData.Models)
            {
                var occupancy = await _assetOccupancyManager.CreateAsync(model, eventData.OccupierUserId);

                await _assetOccupancyRepository.InsertAsync(occupancy);
            
                occupancies.Add(occupancy);
            }
        
            foreach (var model in eventData.ByCategoryModels)
            {
                var occupancy = await _assetOccupancyManager.CreateByCategoryIdAsync(model, eventData.OccupierUserId);

                await _assetOccupancyRepository.InsertAsync(occupancy);
                
                occupancies.Add(occupancy);
            }
            
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

    protected virtual async Task CreatePublishSuccessResultEventAsync(AssetOccupancy occupancy, OccupyAssetByCategoryEto inputEto)
    {
        var resultEto = new AssetOccupancyResultEto(occupancy.TenantId, inputEto.RequestId, true,
            MapToAssetOccupancyInfoModel(occupancy));
        
        inputEto.MapExtraPropertiesTo(resultEto, MappingPropertyDefinitionChecks.None);
        
        await _distributedEventBus.PublishAsync(resultEto);
    }

    protected virtual async Task CreatePublishSuccessResultEventAsync(List<AssetOccupancy> occupancies, BulkOccupyAssetEto inputEto)
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