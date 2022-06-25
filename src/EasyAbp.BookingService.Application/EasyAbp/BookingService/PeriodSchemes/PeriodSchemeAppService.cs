using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeAppService :
    CrudAppService<PeriodScheme, PeriodSchemeDto, Guid, GetPeriodSchemesRequestDto, CreatePeriodSchemeDto,
        UpdatePeriodSchemeDto>, IPeriodSchemeAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Delete;

    private readonly IPeriodSchemeRepository _repository;
    private readonly PeriodSchemeManager _periodSchemeManager;

    public PeriodSchemeAppService(IPeriodSchemeRepository repository,
        PeriodSchemeManager periodSchemeManager) : base(repository)
    {
        _repository = repository;
        _periodSchemeManager = periodSchemeManager;
    }

    protected override async Task<IQueryable<PeriodScheme>> CreateFilteredQueryAsync(
        GetPeriodSchemesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query
            .WhereIf(!input.Name.IsNullOrWhiteSpace(),
                x => x.Name == input.Name);
    }

    protected override async Task<PeriodScheme> MapToEntityAsync(CreatePeriodSchemeDto createInput)
    {
        return await _periodSchemeManager.CreateAsync(
            createInput.Name,
            await MapToPeriodEntitiesAsync(createInput.Periods)
        );
    }

    protected override Task MapToEntityAsync(UpdatePeriodSchemeDto input, PeriodScheme entity)
    {
        entity.Update(input.Name);

        return Task.CompletedTask;
    }

    protected virtual async Task<List<Period>> MapToPeriodEntitiesAsync(List<CreateUpdatePeriodDto> createInput)
    {
        var periods = new List<Period>();

        foreach (var createPeriodDto in createInput)
        {
            periods.Add(
                await _periodSchemeManager.CreatePeriodAsync(createPeriodDto.StartingTime, createPeriodDto.Duration));
        }

        return periods;
    }

    public virtual async Task<PeriodSchemeDto> SetAsDefaultAsync(Guid id)
    {
        await CheckUpdatePolicyAsync();

        var scheme = await GetEntityByIdAsync(id);

        var defaultScheme = await _repository.FindDefaultSchemeAsync();

        if (defaultScheme is not null)
        {
            await _periodSchemeManager.UnsetDefaultAsync(defaultScheme);

            await _repository.UpdateAsync(defaultScheme, true);
        }

        await _periodSchemeManager.SetAsDefaultAsync(scheme);

        await _repository.UpdateAsync(scheme, true);

        return await MapToGetOutputDtoAsync(scheme);
    }

    public virtual async Task<PeriodSchemeDto> CreatePeriodAsync(Guid periodSchemeId, CreateUpdatePeriodDto input)
    {
        var periodScheme = await GetEntityByIdAsync(periodSchemeId);

        var period = await _periodSchemeManager.CreatePeriodAsync(input.StartingTime, input.Duration);

        periodScheme.Periods.Add(period);

        await _repository.UpdateAsync(periodScheme, true);

        return await MapToGetOutputDtoAsync(periodScheme);
    }

    public virtual async Task<PeriodSchemeDto> UpdatePeriodAsync(Guid periodSchemeId, Guid periodId,
        CreateUpdatePeriodDto input)
    {
        var periodScheme = await GetEntityByIdAsync(periodSchemeId);

        await _periodSchemeManager.UpdatePeriodAsync(periodScheme, periodId, input.StartingTime, input.Duration);

        await _repository.UpdateAsync(periodScheme, true);

        return await MapToGetOutputDtoAsync(periodScheme);
    }

    public virtual async Task<PeriodSchemeDto> DeletePeriodAsync(Guid periodSchemeId, Guid periodId)
    {
        var periodScheme = await GetEntityByIdAsync(periodSchemeId);

        await _periodSchemeManager.DeletePeriodAsync(periodScheme, periodId);

        await _repository.UpdateAsync(periodScheme, true);

        return await MapToGetOutputDtoAsync(periodScheme);
    }

    public override async Task DeleteAsync(Guid id)
    {
        await CheckDeletePolicyAsync();
        var entity = await GetEntityByIdAsync(id);
        if (await _periodSchemeManager.IsPeriodSchemeInUseAsync(entity))
        {
            throw new CannotDeletePeriodSchemeInUseException(entity.Id, entity.Name);
        }

        await _repository.DeleteAsync(entity, true);
    }

    protected override async Task<PeriodSchemeDto> MapToGetOutputDtoAsync(PeriodScheme entity)
    {
        var dto = await base.MapToGetOutputDtoAsync(entity);

        dto.Periods = dto.Periods.OrderBy(x => x.StartingTime).ThenBy(x => x.Duration).ToList();

        return dto;
    }
}