using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.ViewModels;
using JetBrains.Annotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetCategories.AssetCategory.ViewModels;

public class EditAssetCategoryViewModel : ExtensibleObject
{
    [Display(Name = "AssetCategoryPeriodSchemeId")]
    public Guid? PeriodSchemeId { get; set; }

    [Display(Name = "AssetCategoryDefaultPeriodUsable")]
    public PeriodUsable? DefaultPeriodUsable { get; set; }

    [Display(Name = "AssetCategoryTimeInAdvance")]
    [CanBeNull]
    public TimeInAdvanceViewModel TimeInAdvance { get; set; }

    [Display(Name = "AssetCategoryDisabled")]
    public bool Disabled { get; set; }

    [Display(Name = "AssetCategoryParentId")]
    public Guid? ParentId { get; set; }

    [Display(Name = "AssetCategoryDisplayName")]
    public string DisplayName { get; set; }
}