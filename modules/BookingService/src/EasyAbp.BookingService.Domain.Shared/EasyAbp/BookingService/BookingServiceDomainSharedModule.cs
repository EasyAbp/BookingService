using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceCommonDomainSharedModule)
)]
public class BookingServiceDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}