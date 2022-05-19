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
            .WhereIf(input.StartingDateTime.HasValue,
                x => x.StartingDateTime >= input.StartingDateTime.Value)
            .WhereIf(input.EndingDateTime.HasValue,
                x => x.EndingDateTime <= input.EndingDateTime.Value);
    }

    protected override async Task<AssetSchedule> MapToEntityAsync(CreateUpdateAssetScheduleDto createInput)
    {
        return await _assetScheduleManager.CreateAsync(
            createInput.AssetId,
            createInput.StartingDateTime,
            createInput.EndingDateTime,
            createInput.PeriodUsable,
            ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(createInput.TimeInAdvance)
        );
    }

    protected override async Task MapToEntityAsync(CreateUpdateAssetScheduleDto updateInput, AssetSchedule entity)
    {
        await _assetScheduleManager.UpdateAsync(entity,
            updateInput.AssetId,
            updateInput.StartingDateTime,
            updateInput.EndingDateTime,
            updateInput.PeriodUsable,
            ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(updateInput.TimeInAdvance)
        );
    }
}