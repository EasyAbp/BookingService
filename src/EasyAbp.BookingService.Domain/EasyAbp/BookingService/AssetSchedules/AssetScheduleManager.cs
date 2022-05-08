using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleManager : DomainService, IAssetScheduleManager, IUnitOfWorkEnabled
{
    private readonly IAssetScheduleRepository _repository;
    private readonly IAssetScheduleSelector _assetScheduleSelector;

    public AssetScheduleManager(IAssetScheduleRepository repository,
        IAssetScheduleSelector assetScheduleSelector)
    {
        _repository = repository;
        _assetScheduleSelector = assetScheduleSelector;
    }

    public virtual async Task<AssetSchedule> CreateAsync(Guid assetId,
        DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance)
    {
        // Check schedule duplication
        var schedule = await GetAssetScheduleAsync(assetId, date, startingTime, duration);
        if (schedule is not null)
        {
            throw new AssetScheduleExistsException(assetId, date, startingTime, duration);
        }

        return new AssetSchedule(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            assetId,
            date,
            startingTime,
            duration,
            schedulePolicy,
            timeInAdvance);
    }

    public virtual async Task UpdateAsync(AssetSchedule entity, Guid assetId, DateTime date, TimeSpan startingTime,
        TimeSpan duration, AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance)
    {
        // Check schedule duplication
        var schedule = await GetAssetScheduleAsync(assetId, date, startingTime, duration);
        if (schedule is not null)
        {
            throw new AssetScheduleExistsException(assetId, date, startingTime, duration);
        }

        entity.Update(assetId, date, startingTime, duration, schedulePolicy, timeInAdvance);
    }

    /// <inheritdoc cref="IAssetScheduleManager"/>
    public virtual async Task<AssetSchedule> GetAssetScheduleAsync(Guid assetId,
        DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        var assetSchedules = await _repository.GetAssetScheduleListAfterDateAsync(assetId, date);

        return await _assetScheduleSelector.SelectAsync(assetSchedules, date, startingTime, duration);
    }
}