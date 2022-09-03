using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceApplicationModule),
    typeof(BookingServiceDomainTestModule)
    )]
public class BookingServiceApplicationTestModule : AbpModule
{

}
