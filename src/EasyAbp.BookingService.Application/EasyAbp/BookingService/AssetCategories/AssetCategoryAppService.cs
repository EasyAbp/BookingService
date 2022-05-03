using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.Permissions;
using EasyAbp.BookingService.AssetCategories.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategoryAppService : CrudAppService<AssetCategory, AssetCategoryDto, Guid,
        GetAssetCategoriesRequestDto, CreateUpdateAssetCategoryDto, CreateUpdateAssetCategoryDto>,
    IAssetCategoryAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.AssetCategory.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.AssetCategory.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.AssetCategory.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.AssetCategory.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.AssetCategory.Delete;

    private readonly IAssetCategoryRepository _repository;

    public AssetCategoryAppService(IAssetCategoryRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<AssetCategory>> CreateFilteredQueryAsync(
        GetAssetCategoriesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query.WhereIf(input.Disabled.HasValue,
                x => x.Disabled == input.Disabled.Value)
            .WhereIf(!input.DisplayName.IsNullOrWhiteSpace(),
                x => x.DisplayName == input.DisplayName)
            .WhereIf(!input.AssetDefinitionName.IsNullOrWhiteSpace(),
                x => x.AssetDefinitionName == input.AssetDefinitionName);
    }
}