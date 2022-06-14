using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class OccupyAssetByCategoryInfoModel : IOccupyingBaseInfo
{
    public Guid AssetCategoryId { get; set; }

    public int Volume { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    protected OccupyAssetByCategoryInfoModel()
    {
    }

    public OccupyAssetByCategoryInfoModel(
        Guid assetCategoryId, int volume, DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        AssetCategoryId = assetCategoryId;
        Volume = volume;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
    }
}