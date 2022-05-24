using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.Assets.Dtos;
using EasyAbp.BookingService.Dtos;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.Assets;

public class AssetAppService : CrudAppService<Asset, AssetDto, Guid, GetAssetsRequestDto, CreateUpdateAssetDto,
        CreateUpdateAssetDto>,
    IAssetAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.Asset.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.Asset.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.Asset.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.Asset.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.Asset.Delete;

    private readonly IAssetRepository _repository;
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly AssetManager _assetManager;

    public AssetAppService(IAssetRepository repository,
        IAssetCategoryRepository assetCategoryRepository,
        AssetManager assetManager) : base(repository)
    {
        _repository = repository;
        _assetCategoryRepository = assetCategoryRepository;
        _assetManager = assetManager;
    }

    protected override async Task<IQueryable<Asset>> CreateFilteredQueryAsync(
        GetAssetsRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query.WhereIf(input.AssetCategoryId.HasValue,
                x => x.AssetCategoryId == input.AssetCategoryId.Value)
            .WhereIf(input.Disabled.HasValue,
                x => x.Disabled == input.Disabled.Value)
            .WhereIf(!input.Name.IsNullOrWhiteSpace(),
                x => x.Name == input.Name)
            .WhereIf(!input.AssetDefinitionName.IsNullOrWhiteSpace(),
                x => x.AssetDefinitionName == input.AssetDefinitionName);
    }

    protected override async Task<Asset> MapToEntityAsync(CreateUpdateAssetDto createInput)
    {
        var category = await _assetCategoryRepository.GetAsync(createInput.AssetCategoryId);

        return await _assetManager.CreateAsync(
            createInput.Name,
            createInput.AssetDefinitionName,
            category,
            createInput.PeriodSchemeId,
            createInput.DefaultPeriodUsable,
            createInput.Priority,
            ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(createInput.TimeInAdvance),
            createInput.Disabled);
    }

    protected override async Task MapToEntityAsync(CreateUpdateAssetDto updateInput, Asset entity)
    {
        var category = await _assetCategoryRepository.GetAsync(updateInput.AssetCategoryId);
        
        await _assetManager.UpdateAsync(entity,
            updateInput.Name,
            updateInput.AssetDefinitionName,
            category,
            updateInput.PeriodSchemeId,
            updateInput.DefaultPeriodUsable,
            updateInput.Priority,
            ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(updateInput.TimeInAdvance),
            updateInput.Disabled);
    }
}