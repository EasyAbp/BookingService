using System;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetSchedules.Dtos;

[Serializable]
public class CreateUpdateAssetScheduleDto : ExtensibleObject
{
    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public AssetSchedulePolicy SchedulePolicy { get; set; }

    public TimeInAdvanceDto TimeInAdvance { get; set; }
}