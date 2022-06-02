using System;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetCategories.Dtos;

[Serializable]
public class UpdateAssetCategoryDto : ExtensibleObject
{
    public Guid? PeriodSchemeId { get; set; }

    public PeriodUsable? DefaultPeriodUsable { get; set; }

    public TimeInAdvanceDto TimeInAdvance { get; set; }

    public bool Disabled { get; set; }

    public Guid? ParentId { get; set; }

    public string DisplayName { get; set; }
}