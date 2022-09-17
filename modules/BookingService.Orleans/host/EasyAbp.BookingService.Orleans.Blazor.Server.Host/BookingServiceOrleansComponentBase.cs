using EasyAbp.BookingService.Localization;
using Volo.Abp.AspNetCore.Components;

namespace EasyAbp.BookingService.Blazor.Server.Host;

public abstract class BookingServiceOrleansComponentBase : AbpComponentBase
{
    protected BookingServiceOrleansComponentBase()
    {
        LocalizationResource = typeof(BookingServiceResource);
    }
}
