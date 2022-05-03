using System;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.Assets.Dtos;

[Serializable]
public class AssetDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public string AssetDefinitionName { get; set; }

    public Guid AssetCategoryId { get; set; }

    public Guid? PeriodSchemeId { get; set; }

    public AssetSchedulePolicy? DefaultSchedulePolicy { get; set; }

    public int Priority { get; set; }

    public TimeInAdvanceDto TimeInAdvance { get; set; }

    public bool Disabled { get; set; }
}