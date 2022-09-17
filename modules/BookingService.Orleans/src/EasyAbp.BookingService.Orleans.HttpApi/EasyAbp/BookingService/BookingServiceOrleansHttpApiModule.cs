using Localization.Resources.AbpUi;
using EasyAbp.BookingService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceOrleansApplicationContractsModule),
    typeof(BookingServiceCommonHttpApiModule),
    typeof(AbpAspNetCoreMvcModule))]
public class BookingServiceOrleansHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(BookingServiceOrleansHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<BookingServiceResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}