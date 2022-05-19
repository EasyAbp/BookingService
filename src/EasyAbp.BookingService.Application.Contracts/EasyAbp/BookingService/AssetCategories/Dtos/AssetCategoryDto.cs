using System;
using System.Collections.Generic;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetCategories.Dtos;

[Serializable]
public class AssetCategoryDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public string AssetDefinitionName { get; set; }

    public Guid? PeriodSchemeId { get; set; }

    public PeriodUsable? DefaultPeriodUsable { get; set; }

    public TimeInAdvanceDto TimeInAdvance { get; set; }

    public bool Disabled { get; set; }

    public string Code { get; set; }

    public int Level { get; set; }

    public Guid? ParentId { get; set; }

    public ICollection<AssetCategoryDto> Children { get; set; }

    public string DisplayName { get; set; }
}