﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeAppService : CrudAppService<PeriodScheme, PeriodSchemeDto, Guid, GetPeriodSchemesRequestDto
        , CreateUpdatePeriodSchemeDto, CreateUpdatePeriodSchemeDto>,
    IPeriodSchemeAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Delete;

    private readonly IPeriodSchemeRepository _repository;
    private readonly DefaultPeriodSchemeStore _defaultPeriodSchemeStore;
    private readonly PeriodSchemeManager _periodSchemeManager;

    public PeriodSchemeAppService(IPeriodSchemeRepository repository,
        DefaultPeriodSchemeStore defaultPeriodSchemeStore,
        PeriodSchemeManager periodSchemeManager) : base(repository)
    {
        _repository = repository;
        _defaultPeriodSchemeStore = defaultPeriodSchemeStore;
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

    protected override async Task<PeriodScheme> MapToEntityAsync(CreateUpdatePeriodSchemeDto createInput)
    {
        return await _periodSchemeManager.CreateAsync(
            createInput.Name,
            ObjectMapper.Map<List<CreatePeriodDto>, List<Period>>(createInput.Periods)
        );
    }

    protected override async Task MapToEntityAsync(CreateUpdatePeriodSchemeDto updateInput, PeriodScheme entity)
    {
        await _periodSchemeManager.UpdateAsync(entity,
            updateInput.Name,
            ObjectMapper.Map<List<CreatePeriodDto>, List<Period>>(updateInput.Periods)
        );
    }

    public virtual async Task<PeriodSchemeDto> SetAsDefaultAsync(Guid id)
    {
        await CheckUpdatePolicyAsync();

        var entity = await _repository.GetAsync(id);
        if (entity.IsDefault)
        {
            return await MapToGetOutputDtoAsync(entity);
        }

        var defaultPeriodScheme = await _defaultPeriodSchemeStore.GetAsync();
        if (defaultPeriodScheme is not null)
        {
            await _defaultPeriodSchemeStore.ClearAsync();
            defaultPeriodScheme.UpdateIsDefault(false);
            await _repository.UpdateAsync(defaultPeriodScheme);
        }

        entity.UpdateIsDefault(true);
        await _repository.UpdateAsync(entity);
        return await MapToGetOutputDtoAsync(entity);
    }
}