using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceOrleansDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(BookingServiceCommonApplicationContractsModule)
    )]
public class BookingServiceOrleansApplicationContractsModule : AbpModule
{

}
