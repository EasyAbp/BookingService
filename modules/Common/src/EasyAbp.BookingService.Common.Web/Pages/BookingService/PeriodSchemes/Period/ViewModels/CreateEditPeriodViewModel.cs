using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.Period.ViewModels;

public class CreateEditPeriodViewModel : ExtensibleObject, IPeriodInfo
{
    [Display(Name = "PeriodStartingTime")]
    public TimeSpan StartingTime { get; set; }

    [Display(Name = "PeriodDuration")]
    public TimeSpan Duration { get; set; }
}