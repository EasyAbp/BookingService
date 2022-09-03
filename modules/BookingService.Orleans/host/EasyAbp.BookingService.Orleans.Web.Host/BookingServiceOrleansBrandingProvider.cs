using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.BookingService;

[Dependency(ReplaceServices = true)]
public class BookingServiceOrleansBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BookingService";
}
