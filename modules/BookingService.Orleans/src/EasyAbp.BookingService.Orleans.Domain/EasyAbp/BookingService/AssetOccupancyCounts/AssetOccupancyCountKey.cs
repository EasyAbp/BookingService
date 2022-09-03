using System;

namespace EasyAbp.BookingService.AssetOccupancyCounts;

public class AssetOccupancyCountKey
{
    public DateTime Date { get; set; }

    public Guid AssetId { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    protected AssetOccupancyCountKey()
    {
    }

    public AssetOccupancyCountKey(DateTime date, Guid assetId, TimeSpan startingTime, TimeSpan duration)
    {
        Date = date;
        AssetId = assetId;
        StartingTime = startingTime;
        Duration = duration;
    }
}