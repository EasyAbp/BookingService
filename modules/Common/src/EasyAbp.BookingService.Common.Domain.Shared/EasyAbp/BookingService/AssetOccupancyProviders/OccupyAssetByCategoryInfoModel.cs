using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class OccupyAssetByCategoryInfoModel : IOccupyingBaseInfo
{
    public Guid AssetCategoryId { get; set; }

    public Guid? PeriodSchemeId { get; set; }

    public int Volume { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    protected OccupyAssetByCategoryInfoModel()
    {
    }

    public OccupyAssetByCategoryInfoModel(
        Guid assetCategoryId, Guid? periodSchemeId, int volume, DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        AssetCategoryId = assetCategoryId;
        PeriodSchemeId = periodSchemeId;
        Volume = volume;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
    }
}