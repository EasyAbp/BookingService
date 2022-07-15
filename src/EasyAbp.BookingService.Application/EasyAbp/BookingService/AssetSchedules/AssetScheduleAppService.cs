using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using EasyAbp.BookingService.Dtos;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleAppService : CrudAppService<AssetSchedule, AssetScheduleDto, Guid,
        GetAssetSchedulesRequestDto, CreateAssetScheduleDto, UpdateAssetScheduleDto>,
    IAssetScheduleAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.AssetSchedule.Delete;

    private readonly IAssetScheduleRepository _repository;
    private readonly IAssetRepository _assetRepository;
    private readonly AssetScheduleManager _assetScheduleManager;


    public AssetScheduleAppService(IAssetScheduleRepository repository,
        IAssetRepository assetRepository,
        AssetScheduleManager assetScheduleManager) : base(repository)
    {
        _repository = repository;
        _assetRepository = assetRepository;
        _assetScheduleManager = assetScheduleManager;
    }

    protected override async Task<IQueryable<AssetSchedule>> CreateFilteredQueryAsync(
        GetAssetSchedulesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        query = query.WhereIf(input.Date.HasValue,
                x => x.Date == input.Date.Value)
            .WhereIf(input.AssetId.HasValue,
                x => x.AssetId == input.AssetId.Value);

        if (input.AssetCategoryId.HasValue)
        {
            var assetQueryable = await _assetRepository.GetQueryableAsync();
            query = from schedule in query
                from asset in assetQueryable
                where schedule.AssetId == asset.Id && asset.AssetCategoryId == input.AssetCategoryId
                select schedule;
        }

        return query;
    }

    protected override async Task<AssetSchedule> MapToEntityAsync(CreateAssetScheduleDto createInput)
    {
        return await _assetScheduleManager.CreateAsync(
            createInput.Date,
            createInput.AssetId,
            createInput.PeriodSchemeId,
            createInput.PeriodId,
            createInput.PeriodUsable,
            ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(createInput.TimeInAdvance)
        );
    }

    protected override Task MapToEntityAsync(UpdateAssetScheduleDto updateInput, AssetSchedule entity)
    {
        entity.Update(
            updateInput.PeriodUsable,
            ObjectMapper.Map<TimeInAdvanceDto, TimeInAdvance>(updateInput.TimeInAdvance)
        );

        return Task.CompletedTask;
    }
}