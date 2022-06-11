using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class PeriodOccupancyModel
{
    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan EndingTime { get; set; }

    public Guid PeriodSchemeId { get; set; }

    public Guid PeriodId { get; set; }
    
    public bool Available { get; set; }

    public PeriodOccupancyModel(
        DateTime date,
        TimeSpan startingTime,
        TimeSpan endingTime,
        Guid periodSchemeId,
        Guid periodId,
        bool available)
    {
        Date = date;
        StartingTime = startingTime;
        EndingTime = endingTime;
        PeriodSchemeId = periodSchemeId;
        PeriodId = periodId;
        Available = available;
    }

    public bool IsIntersected(DateTime date, TimeSpan targetStartingTime, TimeSpan targetEndingTime)
    {
        if (date != Date)
        {
            return false;
        }
        
        return !(targetStartingTime >= EndingTime || targetEndingTime <= StartingTime);
    }

    public DateTime GetStartingDateTime() => Date + StartingTime;
}