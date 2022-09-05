using EasyAbp.Abp.Trees.EntityFrameworkCore;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.PeriodSchemes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[DependsOn(
    typeof(BookingServiceCommonDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpTreesEntityFrameworkCoreModule)
)]
public class BookingServiceCommonEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BookingServiceCommonDbContext>(AddCommonRepositories);
    }

    public static void AddCommonRepositories(IAbpDbContextRegistrationOptionsBuilder options)
    {
        options.AddRepository<AssetCategory, AssetCategoryRepository>();
        options.AddRepository<AssetOccupancy, AssetOccupancyRepository>();
        options.AddRepository<AssetPeriodScheme, AssetPeriodSchemeRepository>();
        options.AddRepository<Asset, AssetRepository>();
        options.AddRepository<PeriodScheme, PeriodSchemeRepository>();

        options.Entity<AssetCategory>(entityOptions =>
            entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
        options.Entity<AssetOccupancy>(entityOptions =>
            entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
        options.Entity<AssetPeriodScheme>(entityOptions =>
            entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
        options.Entity<Asset>(entityOptions =>
            entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
        options.Entity<PeriodScheme>(entityOptions =>
            entityOptions.DefaultWithDetailsFunc = x => x.IncludeDetails());
    }
}