using Volo.Abp.Studio;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpStudioModuleInstallerModule),
    typeof(AbpVirtualFileSystemModule)
    )]
public class BookingServiceInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BookingServiceInstallerModule>();
        });
    }
}
