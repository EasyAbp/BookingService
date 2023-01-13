using EasyAbp.Abp.Trees;
using Orleans.Serialization;
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
public class BookingServiceOrleansDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // todo: upgrade to Orleans 7
        // context.Services.AddSerializer(builder =>
        // {
        //     builder.AddJsonSerializer(isSupported: type =>
        //         type.Namespace?.StartsWith("EasyAbp.BookingService") ?? false);
        // });
    }
}