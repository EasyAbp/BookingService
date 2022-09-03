using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceCommonApplicationModule),
    typeof(BookingServiceCommonDomainTestModule)
    )]
public class BookingServiceCommonApplicationTestModule : AbpModule
{

}
