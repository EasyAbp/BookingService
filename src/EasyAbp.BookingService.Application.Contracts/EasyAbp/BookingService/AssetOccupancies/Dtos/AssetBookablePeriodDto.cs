using System;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class AssetBookablePeriodDto : IHasPeriodInfo
{
    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }
}