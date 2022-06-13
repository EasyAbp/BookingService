using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public interface IOccupyingTimeInfo : IPeriodInfo
{
    DateTime Date { get; }
}