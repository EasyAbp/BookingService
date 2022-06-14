using EasyAbp.Abp.Trees.EntityFrameworkCore;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancyCounts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[DependsOn(
    typeof(BookingServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpTreesEntityFrameworkCoreModule)
)]
public class BookingServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BookingServiceDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<AssetCategory, AssetCategoryRepository>();
            options.AddRepository<AssetOccupancy, AssetOccupancyRepository>();
            options.AddRepository<AssetOccupancyCount, AssetOccupancyCountRepository>();
            options.AddRepository<AssetPeriodScheme, AssetPeriodSchemeRepository>();
            options.AddRepository<Asset, AssetRepository>();
            options.AddRepository<AssetSchedule, AssetScheduleRepository>();
            options.AddRepository<PeriodScheme, PeriodSchemeRepository>();

            options.Entity<AssetCategory>(entityOptions =>
                entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
            options.Entity<AssetOccupancy>(entityOptions =>
                entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
            options.Entity<AssetPeriodScheme>(entityOptions =>
                entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
            options.Entity<Asset>(entityOptions =>
                entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
            options.Entity<AssetSchedule>(entityOptions =>
                entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
            options.Entity<PeriodScheme>(entityOptions =>
                entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
        });
    }
}