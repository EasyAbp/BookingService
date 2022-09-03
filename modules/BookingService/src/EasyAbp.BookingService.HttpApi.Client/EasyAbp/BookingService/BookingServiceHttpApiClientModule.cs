using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(BookingServiceApplicationContractsModule),
    typeof(BookingServiceCommonHttpApiClientModule),
    typeof(AbpHttpClientModule))]
public class BookingServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(BookingServiceApplicationContractsModule).Assembly,
            BookingServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BookingServiceHttpApiClientModule>();
        });
    }
}