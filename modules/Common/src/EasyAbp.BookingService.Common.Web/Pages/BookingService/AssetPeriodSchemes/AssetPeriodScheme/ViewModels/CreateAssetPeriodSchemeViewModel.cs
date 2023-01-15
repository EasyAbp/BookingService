using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetPeriodSchemes.AssetPeriodScheme.ViewModels;

public class CreateAssetPeriodSchemeViewModel : ExtensibleObject
{
    [Display(Name = "AssetPeriodSchemePeriodSchemeId")]
    public Guid PeriodSchemeId { get; set; }

    [Display(Name = "AssetPeriodSchemeAssetId")]
    public Guid AssetId { get; set; }

    [Display(Name = "AssetPeriodSchemeDate")]
    public DateTime Date { get; set; }
}