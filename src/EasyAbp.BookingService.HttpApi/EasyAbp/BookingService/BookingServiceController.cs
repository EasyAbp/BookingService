using EasyAbp.BookingService.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.BookingService;

[Area(BookingServiceRemoteServiceConsts.ModuleName)]
public abstract class BookingServiceController : AbpControllerBase
{
    protected BookingServiceController()
    {
        LocalizationResource = typeof(BookingServiceResource);
    }
}
