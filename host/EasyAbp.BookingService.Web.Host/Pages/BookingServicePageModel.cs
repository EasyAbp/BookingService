using EasyAbp.BookingService.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EasyAbp.BookingService.Pages;

public abstract class BookingServicePageModel : AbpPageModel
{
    protected BookingServicePageModel()
    {
        LocalizationResourceType = typeof(BookingServiceResource);
    }
}
