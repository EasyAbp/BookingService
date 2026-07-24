using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceOrleansDomainModule),
    typeof(BookingServiceOrleansApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule),
    typeof(BookingServiceCommonApplicationModule)
)]
public class BookingServiceOrleansApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<BookingServiceOrleansApplicationModule>();
    }
}