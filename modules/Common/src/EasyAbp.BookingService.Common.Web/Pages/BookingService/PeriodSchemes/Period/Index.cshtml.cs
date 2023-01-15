using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.Period;

public class IndexModel : BookingServicePageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid PeriodSchemeId { get; set; }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}