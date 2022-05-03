using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.Assets.Dtos;
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

    public AssetAppService(IAssetRepository repository) : base(repository)
    {
        _repository = repository;
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
}