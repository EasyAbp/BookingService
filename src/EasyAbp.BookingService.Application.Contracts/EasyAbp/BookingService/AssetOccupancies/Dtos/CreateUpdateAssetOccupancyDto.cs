using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class CreateUpdateAssetOccupancyDto : ExtensibleObject
{
    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }

    // TODO validate this should less than 24 hours
    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }
}