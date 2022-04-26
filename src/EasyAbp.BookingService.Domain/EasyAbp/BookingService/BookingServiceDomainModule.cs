using EasyAbp.Abp.Trees;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpTreesDomainModule),
    typeof(BookingServiceDomainSharedModule)
)]
public class BookingServiceDomainModule : AbpModule
{

}
