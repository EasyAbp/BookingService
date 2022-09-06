using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Orleans;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleManager : DomainService, IAssetScheduleManager
{
    private readonly IAssetScheduleRepository _repository;
    private readonly IGrainFactory _grainFactory;

    public AssetScheduleManager(IAssetScheduleRepository repository,
        IGrainFactory grainFactory)
    {
        _repository = repository;
        _grainFactory = grainFactory;
    }

    [UnitOfWork]
    public virtual async Task<AssetSchedule> CreateAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        Guid periodId, PeriodUsable periodUsable, [CanBeNull] TimeInAdvance timeInAdvance)
    {
        if (await _repository.FindAsync(date, assetId, periodSchemeId, periodId) is not null)
        {
            throw new AssetScheduleExistsException(date, assetId, periodSchemeId, periodId);
        }

        return new AssetSchedule(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            date,
            assetId,
            periodSchemeId,
            periodId,
            periodUsable,
            timeInAdvance);
    }

    public virtual Task<List<IAssetSchedule>> GetListAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        CancellationToken token = default)
    {
        var grain = _grainFactory.GetGrain<IAssetScheduleGrain>(assetId, CalculateCompoundKey(date));
        return grain.GetListAsync(periodSchemeId);
    }

    [UnitOfWork]
    public virtual Task<bool> AnyByPeriodSchemeIdAsync(Guid periodSchemeId, CancellationToken token = default)
    {
        return _repository.AnyAsync(x => x.PeriodSchemeId == periodSchemeId, token);
    }

    [UnitOfWork]
    public Task<bool> AnyByPeriodIdAsync(Guid periodId, CancellationToken token = default)
    {
        return _repository.AnyAsync(x => x.PeriodId == periodId, token);
    }

    protected virtual string CalculateCompoundKey(DateTime date)
    {
        return AssetScheduleExtensions.CalculateCompoundKey(date, CurrentTenant.Id);
    }
}