using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.Permissions;
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
    private readonly IAssetOccupancyManager _assetOccupancyManager;

    public AssetOccupancyAppService(IAssetOccupancyRepository repository,
        IAssetOccupancyManager assetOccupancyManager) : base(repository)
    {
        _repository = repository;
        _assetOccupancyManager = assetOccupancyManager;
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

    protected override async Task<AssetOccupancy> MapToEntityAsync(CreateUpdateAssetOccupancyDto createInput)
    {
        return await _assetOccupancyManager.CreateAsync(
            createInput.AssetId,
            createInput.Date,
            createInput.StartingTime,
            createInput.Duration,
            createInput.OccupierUserId);
    }

    protected override async Task MapToEntityAsync(CreateUpdateAssetOccupancyDto updateInput, AssetOccupancy entity)
    {
        await _assetOccupancyManager.UpdateAsync(entity,
            updateInput.AssetId,
            updateInput.Date,
            updateInput.StartingTime,
            updateInput.Duration,
            updateInput.OccupierUserId);
    }
}