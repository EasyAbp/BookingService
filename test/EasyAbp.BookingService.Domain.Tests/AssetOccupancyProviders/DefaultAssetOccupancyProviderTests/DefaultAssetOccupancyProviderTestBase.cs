using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetOccupancyCounts;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.PeriodSchemes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace EasyAbp.BookingService.AssetOccupancyProviders.DefaultAssetOccupancyProviderTests;

public abstract class DefaultAssetOccupancyProviderTestBase : BookingServiceDomainTestBase
{
    protected static readonly AssetDefinition AssetDefinition = new(nameof(AssetDefinition),
        PeriodUsable.Accept,
        new TimeInAdvance
        {
            MaxDaysInAdvance = 5
        });

    protected static readonly AssetDefinition AnotherAssetDefinition = new(nameof(AnotherAssetDefinition),
        default,
        new TimeInAdvance
        {
            MaxDaysInAdvance = 3
        });

    protected readonly AssetCategoryManager AssetCategoryManager;
    protected readonly AssetManager AssetManager;
    protected readonly PeriodSchemeManager PeriodSchemeManager;
    protected readonly IPeriodSchemeRepository PeriodSchemeRepository;
    protected readonly DefaultAssetOccupancyProvider AssetOccupancyProvider;
    protected readonly IAssetPeriodSchemeRepository AssetPeriodSchemeRepository;
    protected const int DefaultPeriodStartingTimeHours = 0;
    protected const int DefaultPeriodDurationHours = 1;
    protected IExternalUserLookupServiceProvider ExternalUserLookupServiceProvider;
    protected IClock Clock;
    protected readonly AssetScheduleManager AssetScheduleManager;
    protected readonly IAssetScheduleRepository AssetScheduleRepository;
    protected readonly IAssetOccupancyCountRepository AssetOccupancyCountRepository;
    protected readonly IAssetRepository AssetRepository;
    protected readonly IAssetCategoryRepository AssetCategoryRepository;

    protected DefaultAssetOccupancyProviderTestBase()
    {
        // ReSharper disable VirtualMemberCallInConstructor
        AssetOccupancyProvider = GetRequiredService<DefaultAssetOccupancyProvider>();

        AssetCategoryManager = GetRequiredService<AssetCategoryManager>();
        AssetManager = GetRequiredService<AssetManager>();
        PeriodSchemeManager = GetRequiredService<PeriodSchemeManager>();
        AssetScheduleManager = GetRequiredService<AssetScheduleManager>();

        AssetScheduleRepository = GetRequiredService<IAssetScheduleRepository>();
        PeriodSchemeRepository = GetRequiredService<IPeriodSchemeRepository>();
        AssetPeriodSchemeRepository = GetRequiredService<IAssetPeriodSchemeRepository>();
        AssetOccupancyCountRepository = GetRequiredService<IAssetOccupancyCountRepository>();
        AssetRepository = GetRequiredService<IAssetRepository>();
        AssetCategoryRepository = GetRequiredService<IAssetCategoryRepository>();
        // ReSharper restore VirtualMemberCallInConstructor
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<BookingServiceOptions>(options =>
        {
            options.AssetDefinitionConfigurations = new List<AssetDefinition>
            {
                AssetDefinition
            };
        });

        ExternalUserLookupServiceProvider = Substitute.For<IExternalUserLookupServiceProvider>();
        services.AddTransient(_ => ExternalUserLookupServiceProvider);
        Clock = Substitute.For<IClock>();
        services.Replace(ServiceDescriptor.Transient(_ => Clock));
    }

    protected virtual Task<AssetCategory> CreateAssetCategoryAsync(bool disabled = default)
    {
        return AssetCategoryManager.CreateAsync(default, nameof(AssetCategory),
            AssetDefinition.Name,
            default,
            default,
            default,
            disabled);
    }

    protected virtual Task<Asset> CreateAssetAsync(AssetCategory assetCategory, bool disabled = default)
    {
        return AssetManager.CreateAsync(nameof(Asset),
            AssetDefinition.Name,
            assetCategory,
            default,
            default,
            1,
            default,
            default,
            disabled);
    }

    protected virtual async Task<PeriodScheme> CreatePeriodScheme()
    {
        return await PeriodSchemeManager.CreateAsync(nameof(PeriodScheme),
            new List<Period>
            {
                await PeriodSchemeManager.CreatePeriodAsync(TimeSpan.FromHours(DefaultPeriodStartingTimeHours),
                    TimeSpan.FromHours(DefaultPeriodDurationHours))
            });
    }
}