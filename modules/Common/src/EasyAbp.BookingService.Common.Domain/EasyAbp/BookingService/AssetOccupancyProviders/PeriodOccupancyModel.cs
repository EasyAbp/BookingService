using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class PeriodOccupancyModel
{
    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan EndingTime { get; set; }

    public Guid PeriodSchemeId { get; set; }

    public Guid PeriodId { get; set; }

    public int TotalVolume { get; set; }

    public int AvailableVolume { get; set; }

    public PeriodOccupancyModel(
        DateTime date,
        TimeSpan startingTime,
        TimeSpan endingTime,
        Guid periodSchemeId,
        Guid periodId,
        int totalVolume,
        int availableVolume)
    {
        Date = date.Date;
        StartingTime = startingTime;
        EndingTime = endingTime;
        PeriodSchemeId = periodSchemeId;
        PeriodId = periodId;
        TotalVolume = totalVolume;
        AvailableVolume = availableVolume;
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

    public DateTime GetEndingDateTime() => Date + EndingTime;
}