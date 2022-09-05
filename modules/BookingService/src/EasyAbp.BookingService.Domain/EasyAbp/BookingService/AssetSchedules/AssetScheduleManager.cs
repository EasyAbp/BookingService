using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleManager : DomainService, IAssetScheduleManager
{
    private readonly IAssetScheduleRepository _repository;

    public AssetScheduleManager(IAssetScheduleRepository repository)
    {
        _repository = repository;
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
    public virtual async Task<List<IAssetSchedule>> GetListAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        CancellationToken token = default)
    {
        var list = await _repository.GetListAsync(date, assetId, periodSchemeId, token);
        return list.Cast<IAssetSchedule>().ToList();
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