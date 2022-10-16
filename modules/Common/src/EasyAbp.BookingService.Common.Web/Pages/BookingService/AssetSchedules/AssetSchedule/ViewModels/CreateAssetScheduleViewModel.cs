using System;
using System.ComponentModel.DataAnnotations;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Web.Pages.BookingService.ViewModels;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetSchedules.AssetSchedule.ViewModels;

public class CreateAssetScheduleViewModel : ExtensibleObject
{
    [Display(Name = "AssetScheduleDate")]
    public DateTime Date { get; set; }

    [Display(Name = "AssetScheduleAssetId")]
    public Guid AssetId { get; set; }

    [Display(Name = "AssetSchedulePeriodSchemeId")]
    public Guid PeriodSchemeId { get; set; }

    [Display(Name = "AssetSchedulePeriodId")]
    public Guid PeriodId { get; set; }

    [Display(Name = "AssetSchedulePeriodUsable")]
    public PeriodUsable PeriodUsable { get; set; }

    [Display(Name = "AssetScheduleTimeInAdvance")]
    public TimeInAdvanceViewModel TimeInAdvance { get; set; }
}