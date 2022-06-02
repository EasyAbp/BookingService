using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class OccupyAssetInfoModel : IHasOccupyingTimeInfo
{
    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    protected OccupyAssetInfoModel()
    {
    }

    public OccupyAssetInfoModel(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        AssetId = assetId;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
    }
}