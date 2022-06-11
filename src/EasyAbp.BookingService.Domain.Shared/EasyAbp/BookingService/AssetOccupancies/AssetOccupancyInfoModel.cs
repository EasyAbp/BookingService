using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyInfoModel : IHasPeriodInfo
{
    public Guid AssetOccupancyId { get; set; }

    public Guid AssetId { get; set; }

    public string Asset { get; set; }

    public string AssetDefinitionName { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }

    public string OccupierName { get; set; }

    public AssetOccupancyInfoModel(Guid assetOccupancyId, Guid assetId, string asset, string assetDefinitionName,
        DateTime date, TimeSpan startingTime, TimeSpan duration, Guid? occupierUserId, string occupierName)
    {
        AssetOccupancyId = assetOccupancyId;
        AssetId = assetId;
        Asset = asset;
        AssetDefinitionName = assetDefinitionName;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        OccupierUserId = occupierUserId;
        OccupierName = occupierName;
    }
}