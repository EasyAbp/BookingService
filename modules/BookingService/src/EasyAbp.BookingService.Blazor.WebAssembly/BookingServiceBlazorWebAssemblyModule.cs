using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.Blazor.WebAssembly;

[DependsOn(
    typeof(BookingServiceBlazorModule),
    typeof(BookingServiceHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class BookingServiceBlazorWebAssemblyModule : AbpModule
{

}
