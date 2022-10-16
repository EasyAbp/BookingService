using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetPeriodSchemes.AssetPeriodScheme.ViewModels;

public class EditAssetPeriodSchemeViewModel : ExtensibleObject
{
    [Display(Name = "AssetPeriodSchemeDate")]
    public DateTime Date { get; set; }
}