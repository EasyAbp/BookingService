using EasyAbp.BookingService.Localization;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService;

public abstract class BookingServiceCommonAppService : ApplicationService
{
    protected BookingServiceCommonAppService()
    {
        LocalizationResource = typeof(BookingServiceResource);
        ObjectMapperContext = typeof(BookingServiceCommonApplicationModule);
    }
}
