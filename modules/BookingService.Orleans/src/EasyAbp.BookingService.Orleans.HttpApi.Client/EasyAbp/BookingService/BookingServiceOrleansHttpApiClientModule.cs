using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceOrleansApplicationContractsModule),
    typeof(BookingServiceCommonHttpApiClientModule),
    typeof(AbpHttpClientModule))]
public class BookingServiceOrleansHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(BookingServiceOrleansApplicationContractsModule).Assembly,
            BookingServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BookingServiceOrleansHttpApiClientModule>();
        });
    }
}