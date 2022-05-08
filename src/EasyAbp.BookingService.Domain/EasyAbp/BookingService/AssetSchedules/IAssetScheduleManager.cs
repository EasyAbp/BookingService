using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleManager
{
    Task<AssetSchedule> CreateAsync(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance);

    Task UpdateAsync(AssetSchedule entity, Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance);

    /// <summary>
    /// Find asset's schedule by date & startingTime & duration
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="date"></param>
    /// <param name="startingTime"></param>
    /// <param name="duration"></param>
    /// <returns>return null if not exists</returns>
    [ItemCanBeNull]
    Task<AssetSchedule> GetAssetScheduleAsync(Guid assetId,
        DateTime date, TimeSpan startingTime, TimeSpan duration);
}