using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(BookingServiceOrleansBlazorModule)
    )]
public class BookingServiceOrleansBlazorServerModule : AbpModule
{

}
