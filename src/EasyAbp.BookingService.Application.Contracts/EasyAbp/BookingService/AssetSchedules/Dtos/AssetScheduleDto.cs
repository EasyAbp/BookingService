using System;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetSchedules.Dtos;

[Serializable]
public class AssetScheduleDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public AssetSchedulePolicy SchedulePolicy { get; set; }

    public TimeInAdvanceDto TimeInAdvance { get; set; }
}