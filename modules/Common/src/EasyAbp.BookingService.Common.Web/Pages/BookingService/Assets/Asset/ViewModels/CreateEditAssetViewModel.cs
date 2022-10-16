using System;
using System.ComponentModel.DataAnnotations;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Web.Pages.BookingService.ViewModels;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.Assets.Asset.ViewModels;

public class CreateEditAssetViewModel : ExtensibleObject
{
    [Display(Name = "AssetName")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "AssetAssetDefinitionName")]
    public string AssetDefinitionName { get; set; }

    [Display(Name = "AssetAssetCategoryId")]
    public Guid AssetCategoryId { get; set; }

    [Display(Name = "AssetPeriodSchemeId")]
    public Guid? PeriodSchemeId { get; set; }

    [Display(Name = "AssetDefaultPeriodUsable")]
    public PeriodUsable? DefaultPeriodUsable { get; set; }

    [Display(Name = "AssetVolume")]
    public int Volume { get; set; }

    [Display(Name = "AssetPriority")]
    public int Priority { get; set; }

    [Display(Name = "AssetTimeInAdvance")]
    public TimeInAdvanceViewModel TimeInAdvance { get; set; }

    [Display(Name = "AssetDisabled")]
    public bool Disabled { get; set; }
}