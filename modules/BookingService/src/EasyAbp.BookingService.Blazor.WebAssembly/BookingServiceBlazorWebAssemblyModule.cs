using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.Blazor.WebAssembly;

[DependsOn(
    typeof(BookingServiceBlazorModule),
    typeof(BookingServiceHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(BookingServiceCommonBlazorWebAssemblyModule)
)]
public class BookingServiceBlazorWebAssemblyModule : AbpModule
{
}