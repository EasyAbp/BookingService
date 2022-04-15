using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using EasyAbp.BookingService.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.BookingService;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class BookingServiceDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BookingServiceDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<BookingServiceResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/EasyAbp/BookingService/Localization");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("EasyAbp.BookingService", typeof(BookingServiceResource));
        });
    }
}
