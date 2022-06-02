using EasyAbp.Abp.Trees;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpTreesDomainModule),
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpCachingModule),
    typeof(BookingServiceDomainSharedModule)
)]
public class BookingServiceDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<BookingServiceOptions>(configuration.GetSection(BookingServiceOptions.ConfigurationKey));
    }
}