using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EasyAbp.BookingService.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class BookingServiceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BookingService";
}
