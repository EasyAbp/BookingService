using EasyAbp.BookingService.Localization;
using Volo.Abp.AspNetCore.Components;

namespace EasyAbp.BookingService.Blazor.Server.Host;

public abstract class BookingServiceComponentBase : AbpComponentBase
{
    protected BookingServiceComponentBase()
    {
        LocalizationResource = typeof(BookingServiceResource);
    }
}
