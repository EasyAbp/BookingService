using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceOrleansApplicationModule),
    typeof(BookingServiceOrleansDomainTestModule)
    )]
public class BookingServiceOrleansApplicationTestModule : AbpModule
{

}
