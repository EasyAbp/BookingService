using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleManager : DomainService
{
    private readonly IAssetScheduleRepository _repository;

    protected IAssetScheduleStore AssetScheduleStore { get; }

    public AssetScheduleManager(IAssetScheduleRepository repository,
        IAssetScheduleStore assetScheduleStore)
    {
        _repository = repository;
        AssetScheduleStore = assetScheduleStore;
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

    [UnitOfWork]
    public virtual Task<List<AssetSchedule>> GetListAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        CancellationToken token = default)
    {
        return AssetScheduleStore.GetListAsync(date, assetId, periodSchemeId, token);
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
}