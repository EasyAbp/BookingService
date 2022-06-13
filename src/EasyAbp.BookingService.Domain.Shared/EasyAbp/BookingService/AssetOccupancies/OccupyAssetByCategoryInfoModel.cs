using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class OccupyAssetByCategoryInfoModel : IHasOccupyingTimeInfo
{
    public Guid AssetCategoryId { get; set; }

    public int Volume { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    protected OccupyAssetByCategoryInfoModel()
    {
    }

    public OccupyAssetByCategoryInfoModel(Guid assetCategoryId, DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        AssetCategoryId = assetCategoryId;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
    }
}