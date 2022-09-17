using EasyAbp.BookingService.Localization;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService;

public abstract class BookingServiceOrleansAppService : ApplicationService
{
    protected BookingServiceOrleansAppService()
    {
        LocalizationResource = typeof(BookingServiceResource);
        ObjectMapperContext = typeof(BookingServiceOrleansApplicationModule);
    }
}
