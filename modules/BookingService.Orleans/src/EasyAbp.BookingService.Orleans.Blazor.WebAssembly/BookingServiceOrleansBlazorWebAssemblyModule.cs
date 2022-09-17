using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.Blazor.WebAssembly;

[DependsOn(
    typeof(BookingServiceOrleansBlazorModule),
    typeof(BookingServiceOrleansHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class BookingServiceOrleansBlazorWebAssemblyModule : AbpModule
{

}
