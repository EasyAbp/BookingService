using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleSelector
{
    [ItemCanBeNull]
    Task<AssetSchedule> SelectAsync(List<AssetSchedule> assetSchedules,
        DateTime date, TimeSpan startingTime, TimeSpan duration);
}