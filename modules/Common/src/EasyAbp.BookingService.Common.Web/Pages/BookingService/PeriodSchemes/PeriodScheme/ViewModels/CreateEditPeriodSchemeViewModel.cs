using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.PeriodScheme.ViewModels;

public class CreateEditPeriodSchemeViewModel : ExtensibleObject
{
    [Display(Name = "PeriodSchemeName")]
    public string Name { get; set; }
}