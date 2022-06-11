using System;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class BookablePeriodDto
{
    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan EndingTime { get; set; }

    public Guid PeriodSchemeId { get; set; }

    public Guid PeriodId { get; set; }

    public bool Available { get; set; }
}