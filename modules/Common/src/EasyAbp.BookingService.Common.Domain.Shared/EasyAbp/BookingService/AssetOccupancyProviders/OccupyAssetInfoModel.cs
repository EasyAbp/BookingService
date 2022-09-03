using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class OccupyAssetInfoModel : IOccupyingBaseInfo
{
    public Guid AssetId { get; set; }

    public int Volume { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    protected OccupyAssetInfoModel()
    {
    }

    public OccupyAssetInfoModel(Guid assetId, int volume, DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        AssetId = assetId;
        Volume = volume;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
    }
}