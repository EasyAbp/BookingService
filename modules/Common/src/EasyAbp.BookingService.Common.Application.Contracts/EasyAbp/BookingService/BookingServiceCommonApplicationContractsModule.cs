using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceCommonDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class BookingServiceCommonApplicationContractsModule : AbpModule
{

}
