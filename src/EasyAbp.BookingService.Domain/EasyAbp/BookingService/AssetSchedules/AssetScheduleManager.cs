using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleManager : DomainService
{
    private readonly IAssetScheduleRepository _repository;

    public AssetScheduleManager(IAssetScheduleRepository repository)
    {
        _repository = repository;
    }

    [UnitOfWork]
    public virtual async Task<AssetSchedule> CreateAsync(Guid assetId,
        DateTime startingDateTime, DateTime endingDateTime,
        PeriodUsable periodUsable, TimeInAdvance timeInAdvance)
    {
        var assetSchedules =
            await _repository.GetAssetScheduleListInScopeAsync(assetId, startingDateTime, endingDateTime, periodUsable);
        if (!assetSchedules.IsNullOrEmpty())
        {
            throw new AssetScheduleExistsException(assetId, startingDateTime, endingDateTime, periodUsable);
        }

        return new AssetSchedule(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            assetId,
            startingDateTime,
            endingDateTime,
            periodUsable,
            timeInAdvance);
    }

    [UnitOfWork]
    public virtual async Task UpdateAsync(AssetSchedule entity, Guid assetId, DateTime startingDateTime,
        DateTime endingDateTime, PeriodUsable periodUsable, TimeInAdvance timeInAdvance)
    {
        var assetSchedules =
            await _repository.GetAssetScheduleListInScopeAsync(assetId, startingDateTime, endingDateTime, periodUsable);
        if (!assetSchedules.IsNullOrEmpty())
        {
            throw new AssetScheduleExistsException(assetId, startingDateTime, endingDateTime, periodUsable);
        }

        entity.Update(assetId, startingDateTime, endingDateTime, periodUsable, timeInAdvance);
    }
}