using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceDomainModule)
)]
public class BookingServiceAssetOccupancyProvidersOrleansModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}