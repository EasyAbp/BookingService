using System;

namespace EasyAbp.BookingService.AssetOccupancies;

[Serializable]
public class AssetBookablePeriod : IHasPeriodInfo
{
    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }
}