using Volo.Abp.Bundling;

namespace EasyAbp.BookingService.Blazor.Host;

public class BookingServiceBlazorHostBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {

    }

    public void AddStyles(BundleContext context)
    {
        context.Add("main.css", true);
    }
}
