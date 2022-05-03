using System;
using System.Threading.Tasks;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleManager
{
    Task<AssetSchedule> CreateAsync(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance);

    Task UpdateAsync(AssetSchedule entity, Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance);
}