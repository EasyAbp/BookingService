using Microsoft.Extensions.DependencyInjection;
using EasyAbp.BookingService.Blazor.Menus;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.BookingService.Blazor;

[DependsOn(
    typeof(BookingServiceCommonApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class BookingServiceCommonBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<BookingServiceCommonBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<BookingServiceCommonBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new BookingServiceCommonMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(BookingServiceCommonBlazorModule).Assembly);
        });
    }
}
