using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceDomainModule),
    typeof(BookingServiceApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule),
    typeof(BookingServiceCommonApplicationModule)
)]
public class BookingServiceApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<BookingServiceApplicationModule>();
    }
}