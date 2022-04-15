using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class BookingServiceApplicationContractsModule : AbpModule
{

}
