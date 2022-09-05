using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using EasyAbp.BookingService.Localization;
using EasyAbp.BookingService.Web.Menus;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using EasyAbp.BookingService.Permissions;

namespace EasyAbp.BookingService.Web;

[DependsOn(
    typeof(BookingServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule),
    typeof(BookingServiceCommonWebModule)
)]
public class BookingServiceWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(BookingServiceResource), typeof(BookingServiceWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(BookingServiceWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new BookingServiceMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<BookingServiceWebModule>(); });

        context.Services.AddAutoMapperObjectMapper<BookingServiceWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BookingServiceWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.
        });
    }
}