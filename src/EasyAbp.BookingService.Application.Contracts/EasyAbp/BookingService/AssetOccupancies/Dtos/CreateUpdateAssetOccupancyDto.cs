using System;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class CreateUpdateAssetOccupancyDto : ExtensibleObject
{
    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }
}