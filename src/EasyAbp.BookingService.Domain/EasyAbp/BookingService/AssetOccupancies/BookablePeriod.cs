using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class BookablePeriod
{
    public TimeSpan StartingTime { get; set; }

    public TimeSpan EndingTime { get; set; }

    public bool Divisible { get; set; }

    public Guid PeriodSchemeId { get; set; }

    public Guid PeriodId { get; set; }

    public bool IsIntersected(TimeSpan targetStartingTime, TimeSpan targetEndingTime)
    {
        return !(targetStartingTime >= EndingTime || targetEndingTime <= StartingTime);
    }
}