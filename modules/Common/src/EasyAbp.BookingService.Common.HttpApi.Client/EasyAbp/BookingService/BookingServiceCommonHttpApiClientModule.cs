using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceCommonApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class BookingServiceCommonHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(BookingServiceCommonApplicationContractsModule).Assembly,
            BookingServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BookingServiceCommonHttpApiClientModule>();
        });

    }
}
