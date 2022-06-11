using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
}