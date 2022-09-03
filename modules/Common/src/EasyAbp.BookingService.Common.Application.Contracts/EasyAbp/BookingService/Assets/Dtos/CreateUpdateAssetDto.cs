using System;
using System.ComponentModel.DataAnnotations;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Assets.Dtos;

[Serializable]
public class CreateUpdateAssetDto : ExtensibleObject
{
    public string Name { get; set; }

    [Required] public string AssetDefinitionName { get; set; }

    public Guid AssetCategoryId { get; set; }

    public Guid? PeriodSchemeId { get; set; }

    public PeriodUsable? DefaultPeriodUsable { get; set; }

    public int Volume { get; set; }

    public int Priority { get; set; }

    public TimeInAdvanceDto TimeInAdvance { get; set; }

    public bool Disabled { get; set; }
}