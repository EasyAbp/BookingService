﻿using Localization.Resources.AbpUi;
using EasyAbp.BookingService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceCommonApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class BookingServiceCommonHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(BookingServiceCommonHttpApiModule).Assembly);
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
