using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(BookingServiceDomainSharedModule)
)]
public class BookingServiceDomainModule : AbpModule
{

}
