using EasyAbp.BookingService.Localization;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService;

public abstract class BookingServiceAppService : ApplicationService
{
    protected BookingServiceAppService()
    {
        LocalizationResource = typeof(BookingServiceResource);
        ObjectMapperContext = typeof(BookingServiceApplicationModule);
    }
}
