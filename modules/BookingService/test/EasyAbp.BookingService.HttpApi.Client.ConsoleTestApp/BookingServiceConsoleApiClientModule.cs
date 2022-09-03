using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BookingServiceHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class BookingServiceConsoleApiClientModule : AbpModule
{

}
