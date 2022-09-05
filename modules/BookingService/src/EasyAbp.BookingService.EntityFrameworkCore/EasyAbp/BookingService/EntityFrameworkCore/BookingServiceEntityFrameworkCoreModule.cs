using EasyAbp.Abp.Trees.EntityFrameworkCore;
using EasyAbp.BookingService.AssetOccupancyCounts;
using EasyAbp.BookingService.AssetSchedules;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[DependsOn(
    typeof(BookingServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpTreesEntityFrameworkCoreModule),
    typeof(BookingServiceCommonEntityFrameworkCoreModule)
)]
public class BookingServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BookingServiceDbContext>(options =>
        {
            BookingServiceCommonEntityFrameworkCoreModule.AddCommonRepositories(options);

            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<AssetSchedule, AssetScheduleRepository>();
            options.AddRepository<AssetOccupancyCount, AssetOccupancyCountRepository>();

            options.Entity<AssetSchedule>(entityOptions =>
                entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
            options.Entity<AssetOccupancyCount>(entityOptions =>
                entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
        });
    }
}