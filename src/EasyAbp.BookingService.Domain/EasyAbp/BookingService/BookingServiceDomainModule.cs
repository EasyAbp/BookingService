using System;
using EasyAbp.Abp.Trees;
using EasyAbp.BookingService.AssetOccupancyProviders;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
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
    typeof(BookingServiceDomainSharedModule)
)]
public class BookingServiceDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<BookingServiceOptions>(configuration.GetSection(BookingServiceOptions.ConfigurationKey));

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.CacheConfigurators.Add(cacheName =>
            {
                if (cacheName == CacheNameAttribute.GetCacheName(typeof(AssetOccupyTransactionCacheItem)))
                {
                    return new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(5)
                    };
                }

                return null;
            });
        });
    }
}