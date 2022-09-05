using EasyAbp.Abp.Trees;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpTreesDomainModule),
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpCachingModule),
    typeof(AbpUsersAbstractionModule),
    typeof(BookingServiceCommonDomainModule)
)]
public class BookingServiceDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}