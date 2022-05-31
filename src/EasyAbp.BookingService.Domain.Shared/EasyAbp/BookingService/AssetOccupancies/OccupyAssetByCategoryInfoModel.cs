using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class OccupyAssetByCategoryInfoModel : IHasPeriodInfo
{
    public Guid AssetCategoryId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }

    protected OccupyAssetByCategoryInfoModel()
    {
    }

    public OccupyAssetByCategoryInfoModel(Guid assetCategoryId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId)
    {
        AssetCategoryId = assetCategoryId;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        OccupierUserId = occupierUserId;
    }
}