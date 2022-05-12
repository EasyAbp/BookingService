using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.Permissions;
using EasyAbp.BookingService.AssetCategories.Dtos;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategoryAppService : CrudAppService<AssetCategory, AssetCategoryDto, Guid,
        GetAssetCategoriesRequestDto, CreateAssetCategoryDto, UpdateAssetCategoryDto>,
    IAssetCategoryAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.AssetCategory.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.AssetCategory.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.AssetCategory.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.AssetCategory.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.AssetCategory.Delete;

    private readonly AssetCategoryManager _assetCategoryManager;
    private readonly IAssetCategoryRepository _repository;

    public AssetCategoryAppService(
        AssetCategoryManager assetCategoryManager,
        IAssetCategoryRepository repository) : base(repository)
    {
        _assetCategoryManager = assetCategoryManager;
        _repository = repository;
    }

    protected override async Task<IQueryable<AssetCategory>> CreateFilteredQueryAsync(
        GetAssetCategoriesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query.Where(x => x.ParentId == input.ParentId)
            .WhereIf(input.Disabled.HasValue,
                x => x.Disabled == input.Disabled.Value)
            .WhereIf(!input.DisplayName.IsNullOrWhiteSpace(),
                x => x.DisplayName == input.DisplayName)
            .WhereIf(!input.AssetDefinitionName.IsNullOrWhiteSpace(),
                x => x.AssetDefinitionName == input.AssetDefinitionName);
    }

    protected override async Task<AssetCategory> MapToEntityAsync(CreateAssetCategoryDto createInput)
    {
        // TODO Check PeriodSchemeId
        return await _assetCategoryManager.CreateAsync(
            createInput.ParentId,
            createInput.DisplayName,
            createInput.AssetDefinitionName,
            createInput.PeriodSchemeId,
            createInput.DefaultSchedulePolicy,
            MapToTimeInAdvance(createInput.TimeInAdvance),
            createInput.Disabled);
    }

    protected override async Task MapToEntityAsync(UpdateAssetCategoryDto updateInput, AssetCategory entity)
    {
        // TODO Check PeriodSchemeId
        await _assetCategoryManager.UpdateAsync(entity,
            updateInput.ParentId,
            updateInput.DisplayName,
            updateInput.PeriodSchemeId,
            updateInput.DefaultSchedulePolicy,
            MapToTimeInAdvance(updateInput.TimeInAdvance),
            updateInput.Disabled);
    }

    protected virtual TimeInAdvance MapToTimeInAdvance(TimeInAdvanceDto input)
    {
        return ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(input);
    }
}