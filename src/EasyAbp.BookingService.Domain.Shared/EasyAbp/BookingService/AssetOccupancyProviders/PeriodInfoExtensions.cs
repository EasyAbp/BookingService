using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public static class PeriodInfoExtensions
{
    public static TimeSpan GetEndingTime(this IPeriodInfo info) => info.StartingTime + info.Duration;
}