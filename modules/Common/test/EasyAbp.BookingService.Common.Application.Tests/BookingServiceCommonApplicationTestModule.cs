using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceCommonApplicationModule),
    typeof(BookingServiceCommonDomainTestModule)
    )]
public class BookingServiceCommonApplicationTestModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        /* Extra properties are only mapped when they are defined for the
         * source and destination types, so define one to test the mapping. */
        OneTimeRunner.Run(() =>
        {
            ObjectExtensionManager.Instance
                .AddOrUpdateProperty<AssetPeriodScheme, string>("MyExtraProperty")
                .AddOrUpdateProperty<AssetPeriodSchemeDto, string>("MyExtraProperty");
        });
    }
}
