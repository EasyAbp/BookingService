using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleAppService : CrudAppService<AssetSchedule, AssetScheduleDto, Guid,
        GetAssetSchedulesRequestDto, CreateUpdateAssetScheduleDto, CreateUpdateAssetScheduleDto>,
    IAssetScheduleAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Delete;

    private readonly IAssetScheduleRepository _repository;

    public AssetScheduleAppService(IAssetScheduleRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<AssetSchedule>> CreateFilteredQueryAsync(
        GetAssetSchedulesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query.WhereIf(input.AssetId.HasValue,
                x => x.AssetId == input.AssetId.Value)
            .WhereIf(input.Date.HasValue,
                x => x.Date == input.Date.Value);
    }
}