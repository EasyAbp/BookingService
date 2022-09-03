using System;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetSchedules.Dtos;

[Serializable]
public class AssetScheduleDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid AssetId { get; set; }

    public Guid PeriodSchemeId { get; set; }

    public Guid PeriodId { get; set; }

    public PeriodUsable PeriodUsable { get; set; }

    public TimeInAdvanceDto TimeInAdvance { get; set; }
}