using EasyAbp.BookingService.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EasyAbp.BookingService.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class BookingServicePageModel : AbpPageModel
{
    protected BookingServicePageModel()
    {
        LocalizationResourceType = typeof(BookingServiceResource);
        ObjectMapperContext = typeof(BookingServiceCommonWebModule);
    }
}
