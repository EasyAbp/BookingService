using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyInfoModel : IHasPeriodInfo
{
    /// <summary>
    /// It will be <c>null</c> if occupancy fails.
    /// </summary>
    public Guid? AssetOccupancyId { get; set; }
    
    public Guid AssetId { get; set; }

    public string AssetName { get; set; }

    public string AssetDefinitionName { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }

    public string OccupierName { get; set; }
}