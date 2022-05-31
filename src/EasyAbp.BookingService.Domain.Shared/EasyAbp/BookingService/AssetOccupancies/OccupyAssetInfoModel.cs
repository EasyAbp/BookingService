using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class OccupyAssetInfoModel : IHasPeriodInfo
{
    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }

    protected OccupyAssetInfoModel()
    {
    }

    public OccupyAssetInfoModel(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId)
    {
        AssetId = assetId;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        OccupierUserId = occupierUserId;
    }
}