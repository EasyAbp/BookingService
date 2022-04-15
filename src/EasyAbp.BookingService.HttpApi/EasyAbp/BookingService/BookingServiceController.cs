using EasyAbp.BookingService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.BookingService;

public abstract class BookingServiceController : AbpControllerBase
{
    protected BookingServiceController()
    {
        LocalizationResource = typeof(BookingServiceResource);
    }
}
