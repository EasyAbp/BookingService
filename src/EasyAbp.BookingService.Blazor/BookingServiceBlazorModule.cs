using Microsoft.Extensions.DependencyInjection;
using EasyAbp.BookingService.Blazor.Menus;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.BookingService.Blazor;

[DependsOn(
    typeof(BookingServiceApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class BookingServiceBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<BookingServiceBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<BookingServiceBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new BookingServiceMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(BookingServiceBlazorModule).Assembly);
        });
    }
}
