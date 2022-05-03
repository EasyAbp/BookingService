using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.Permissions;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyAppService : CrudAppService<AssetOccupancy, AssetOccupancyDto, Guid,
        GetAssetOccupanciesRequestDto, CreateUpdateAssetOccupancyDto, CreateUpdateAssetOccupancyDto>,
    IAssetOccupancyAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Delete;

    private readonly IAssetOccupancyRepository _repository;

    public AssetOccupancyAppService(IAssetOccupancyRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<AssetOccupancy>> CreateFilteredQueryAsync(
        GetAssetOccupanciesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query.WhereIf(input.AssetId.HasValue,
                x => x.AssetId == input.AssetId.Value)
            .WhereIf(input.Date.HasValue,
                x => x.Date == input.Date.Value);
    }
}