using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BookingServiceOrleansHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class BookingServiceOrleansConsoleApiClientModule : AbpModule
{

}
