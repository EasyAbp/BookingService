using System.ComponentModel.DataAnnotations;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Web.Pages.BookingService.ViewModels;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetSchedules.AssetSchedule.ViewModels;

public class EditAssetScheduleViewModel : ExtensibleObject
{
    [Display(Name = "AssetSchedulePeriodUsable")]
    public PeriodUsable PeriodUsable { get; set; }

    [Display(Name = "AssetScheduleTimeInAdvance")]
    public TimeInAdvanceViewModel TimeInAdvance { get; set; }
}