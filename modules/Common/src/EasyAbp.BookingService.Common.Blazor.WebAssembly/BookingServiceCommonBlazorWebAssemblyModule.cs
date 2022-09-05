using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.Blazor.WebAssembly;

[DependsOn(
    typeof(BookingServiceCommonBlazorModule),
    typeof(BookingServiceCommonHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class BookingServiceCommonBlazorWebAssemblyModule : AbpModule
{

}
