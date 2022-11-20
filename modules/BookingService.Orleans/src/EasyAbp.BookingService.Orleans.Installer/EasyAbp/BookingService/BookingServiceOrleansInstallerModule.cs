using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(BookingServiceCommonInstallerModule)
)]
public class BookingServiceOrleansInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BookingServiceOrleansInstallerModule>();
        });
    }
}