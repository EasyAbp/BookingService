using EasyAbp.BookingService.Localization;
using EasyAbp.BookingService.Web.Menus;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.BookingService.Web;

[DependsOn(
    typeof(BookingServiceCommonApplicationContractsModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule)
)]
public class BookingServiceCommonWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(BookingServiceResource),
                typeof(BookingServiceCommonWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(BookingServiceCommonWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new BookingServiceCommonMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BookingServiceCommonWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<BookingServiceCommonWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BookingServiceCommonWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.
        });
    }
}