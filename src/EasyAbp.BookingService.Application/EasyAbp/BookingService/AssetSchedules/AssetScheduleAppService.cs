using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using EasyAbp.BookingService.Dtos;
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
    private readonly AssetScheduleManager _assetScheduleManager;


    public AssetScheduleAppService(IAssetScheduleRepository repository,
        AssetScheduleManager assetScheduleManager) : base(repository)
    {
        _repository = repository;
        _assetScheduleManager = assetScheduleManager;
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

    protected override async Task<AssetSchedule> MapToEntityAsync(CreateUpdateAssetScheduleDto createInput)
    {
        // TODO check assetId exists here?
        return await _assetScheduleManager.CreateAsync(
            createInput.AssetId,
            createInput.Date,
            createInput.StartingTime,
            createInput.Duration,
            createInput.SchedulePolicy,
            ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(createInput.TimeInAdvance)
        );
    }

    protected override async Task MapToEntityAsync(CreateUpdateAssetScheduleDto updateInput, AssetSchedule entity)
    {
        // TODO check assetId exists here?
        await _assetScheduleManager.UpdateAsync(entity,
            updateInput.AssetId,
            updateInput.Date,
            updateInput.StartingTime,
            updateInput.Duration,
            updateInput.SchedulePolicy,
            ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(updateInput.TimeInAdvance)
        );
    }
}