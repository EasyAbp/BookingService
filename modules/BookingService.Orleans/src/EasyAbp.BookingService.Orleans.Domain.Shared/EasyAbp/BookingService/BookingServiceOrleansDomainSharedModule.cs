using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceCommonDomainSharedModule)
)]
public class BookingServiceOrleansDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}