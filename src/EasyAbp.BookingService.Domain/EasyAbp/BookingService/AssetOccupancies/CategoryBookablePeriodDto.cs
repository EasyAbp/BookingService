using System;

namespace EasyAbp.BookingService.AssetOccupancies;

[Serializable]
public class CategoryBookablePeriod : IHasPeriodInfo
{
    public Guid AssetId { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }
}