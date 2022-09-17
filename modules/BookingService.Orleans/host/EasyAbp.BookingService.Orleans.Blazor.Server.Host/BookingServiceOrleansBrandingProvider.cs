using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EasyAbp.BookingService.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class BookingServiceOrleansBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BookingService";
}
