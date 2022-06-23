using System.Collections.Generic;
using EasyAbp.BookingService.AssetDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Guids;

namespace EasyAbp.BookingService;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class BookingServiceApplicationTestBase : BookingServiceTestBase<BookingServiceApplicationTestModule>
{
    protected readonly IGuidGenerator GuidGenerator;

    protected static readonly AssetDefinition AssetDefinition = new(nameof(AssetDefinition),
        default,
        new TimeInAdvance
        {
            MaxDaysInAdvance = 5
        });

    protected static readonly AssetDefinition AnotherAssetDefinition = new(nameof(AnotherAssetDefinition),
        default,
        new TimeInAdvance
        {
            MaxDaysInAdvance = 10
        });

    protected BookingServiceApplicationTestBase()
    {
        GuidGenerator = GetRequiredService<IGuidGenerator>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        services.Configure<BookingServiceOptions>(options =>
        {
            options.AssetDefinitionConfigurations = new List<AssetDefinition>
            {
                AssetDefinition,
                AnotherAssetDefinition
            };
        });
    }
}