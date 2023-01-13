using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.Assets;
using Microsoft.Extensions.DependencyInjection;
using Orleans.TestingHost;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

[Collection(ClusterCollection.Name)]
public class AssetOccupancyGrainTests : BookingServiceOrleansDomainTestBase
{
    private readonly TestCluster _cluster;

    private static readonly AssetDefinition AssetDefinition = new(nameof(AssetDefinition),
        default,
        new TimeInAdvance
        {
            MaxDaysInAdvance = 5
        });

    public AssetOccupancyGrainTests(ClusterFixture fixture)
    {
        _cluster = fixture.Cluster;
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        services.Configure<BookingServiceOptions>(options =>
        {
            options.AssetDefinitionConfigurations = new List<AssetDefinition>
            {
                AssetDefinition
            };
        });
    }

    [Fact]
    public async Task Occupy_Baseline_Test()
    {
        // Arrange
        const string name = nameof(Occupy_Baseline_Test);
        var assetDefinitionName = AssetDefinition.Name;
        var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        var assetManager = GetRequiredService<AssetManager>();
        var assetCategory =
            await assetCategoryManager.CreateAsync(default,
                nameof(AssetCategory),
                AssetDefinition.Name,
                default,
                default,
                default,
                default);
        const int volume = 10;
        const int priority = 10;

        var asset = await assetManager.CreateAsync(name,
            assetDefinitionName,
            assetCategory,
            default,
            default,
            volume,
            priority,
            default,
            default);

        var grain = _cluster.GrainFactory.GetGrain<IAssetOccupancyGrain>(Guid.NewGuid(),
            AssetOccupancyGrainExtensions.CalculateCompoundKey(DateTime.Today, null));
        var model = new ProviderOccupyingInfoModel(asset, assetCategory, TimeSpan.Zero, TimeSpan.FromHours(2),
            DateTime.Today, 1);

        // Act
        await grain.OccupyAsync(model);

        // Assert
        var occupied = await grain.GetAssetOccupanciesAsync();

        occupied.Count.ShouldBe(1);
        occupied[0].Volume.ShouldBe(1);
    }

    [Fact]
    public async Task Occupy_Multiple_Test()
    {
        // Arrange
        const string name = nameof(Occupy_Baseline_Test);
        var assetDefinitionName = AssetDefinition.Name;
        var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        var assetManager = GetRequiredService<AssetManager>();
        var assetCategory =
            await assetCategoryManager.CreateAsync(default,
                nameof(AssetCategory),
                AssetDefinition.Name,
                default,
                default,
                default,
                default);
        const int volume = 10;
        const int priority = 10;

        var asset = await assetManager.CreateAsync(name,
            assetDefinitionName,
            assetCategory,
            default,
            default,
            volume,
            priority,
            default,
            default);

        var grain = _cluster.GrainFactory.GetGrain<IAssetOccupancyGrain>(Guid.NewGuid(),
            AssetOccupancyGrainExtensions.CalculateCompoundKey(DateTime.Today, null));
        var model = new ProviderOccupyingInfoModel(asset, assetCategory, TimeSpan.Zero, TimeSpan.FromHours(2),
            DateTime.Today, 1);

        // Act
        await grain.OccupyAsync(model);
        await grain.OccupyAsync(model);

        // Assert
        var occupied = await grain.GetAssetOccupanciesAsync();

        occupied.Count.ShouldBe(1);
        occupied[0].Volume.ShouldBe(2);
    }

    [Fact]
    public async Task Occupy_InsufficientAssetVolume_Test()
    {
        // Arrange
        const string name = nameof(Occupy_Baseline_Test);
        var assetDefinitionName = AssetDefinition.Name;
        var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        var assetManager = GetRequiredService<AssetManager>();
        var assetCategory =
            await assetCategoryManager.CreateAsync(default,
                nameof(AssetCategory),
                AssetDefinition.Name,
                default,
                default,
                default,
                default);
        const int volume = 10;
        const int priority = 10;

        var asset = await assetManager.CreateAsync(name,
            assetDefinitionName,
            assetCategory,
            default,
            default,
            volume,
            priority,
            default,
            default);

        var grain = _cluster.GrainFactory.GetGrain<IAssetOccupancyGrain>(Guid.NewGuid(),
            AssetOccupancyGrainExtensions.CalculateCompoundKey(DateTime.Today, null));
        var model = new ProviderOccupyingInfoModel(asset, assetCategory, TimeSpan.Zero, TimeSpan.FromHours(2),
            DateTime.Today, 10);

        await grain.OccupyAsync(model);

        // Act & Assert
        await Should.ThrowAsync<InsufficientAssetVolumeException>(() => grain.OccupyAsync(model));
    }

    [Fact]
    public async Task TryRollBackOccupancy_Baseline_Test()
    {
        // Arrange
        const string name = nameof(Occupy_Baseline_Test);
        var assetDefinitionName = AssetDefinition.Name;
        var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        var assetManager = GetRequiredService<AssetManager>();
        var assetCategory =
            await assetCategoryManager.CreateAsync(default,
                nameof(AssetCategory),
                AssetDefinition.Name,
                default,
                default,
                default,
                default);
        const int volume = 10;
        const int priority = 10;

        var asset = await assetManager.CreateAsync(name,
            assetDefinitionName,
            assetCategory,
            default,
            default,
            volume,
            priority,
            default,
            default);

        var grain = _cluster.GrainFactory.GetGrain<IAssetOccupancyGrain>(Guid.NewGuid(),
            AssetOccupancyGrainExtensions.CalculateCompoundKey(DateTime.Today, null));
        var model = new ProviderOccupyingInfoModel(asset, assetCategory, TimeSpan.Zero, TimeSpan.FromHours(2),
            DateTime.Today, 1);
        await grain.OccupyAsync(model);

        // Act
        var result = await grain.TryRollBackOccupancyAsync(new ProviderAssetOccupancyModel(asset.Id, 1, DateTime.Today,
            TimeSpan.Zero, TimeSpan.FromHours(2)));

        // Assert
        var occupied = await grain.GetAssetOccupanciesAsync();

        result.ShouldBeTrue();
        occupied.Count.ShouldBe(1);
        occupied[0].Volume.ShouldBe(0);
    }

    [Fact]
    public async Task TryRollBackOccupancy_Failed_Test()
    {
        // Arrange
        const string name = nameof(Occupy_Baseline_Test);
        var assetDefinitionName = AssetDefinition.Name;
        var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        var assetManager = GetRequiredService<AssetManager>();
        var assetCategory =
            await assetCategoryManager.CreateAsync(default,
                nameof(AssetCategory),
                AssetDefinition.Name,
                default,
                default,
                default,
                default);
        const int volume = 10;
        const int priority = 10;

        var asset = await assetManager.CreateAsync(name,
            assetDefinitionName,
            assetCategory,
            default,
            default,
            volume,
            priority,
            default,
            default);

        var grain = _cluster.GrainFactory.GetGrain<IAssetOccupancyGrain>(Guid.NewGuid(),
            AssetOccupancyGrainExtensions.CalculateCompoundKey(DateTime.Today, null));
        var model = new ProviderOccupyingInfoModel(asset, assetCategory, TimeSpan.Zero, TimeSpan.FromHours(2),
            DateTime.Today, 1);
        await grain.OccupyAsync(model);

        // Act
        var result = await grain.TryRollBackOccupancyAsync(new ProviderAssetOccupancyModel(asset.Id, 2, DateTime.Today,
            TimeSpan.Zero, TimeSpan.FromHours(2)));

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task TryRollBackOccupancy_OccupancyNotExists_Test()
    {
        // Arrange
        const string name = nameof(Occupy_Baseline_Test);
        var assetDefinitionName = AssetDefinition.Name;
        var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        var assetManager = GetRequiredService<AssetManager>();
        var assetCategory =
            await assetCategoryManager.CreateAsync(default,
                nameof(AssetCategory),
                AssetDefinition.Name,
                default,
                default,
                default,
                default);
        const int volume = 10;
        const int priority = 10;

        var asset = await assetManager.CreateAsync(name,
            assetDefinitionName,
            assetCategory,
            default,
            default,
            volume,
            priority,
            default,
            default);

        var grain = _cluster.GrainFactory.GetGrain<IAssetOccupancyGrain>(Guid.NewGuid(),
            AssetOccupancyGrainExtensions.CalculateCompoundKey(DateTime.Today, null));

        // Act
        var result = await grain.TryRollBackOccupancyAsync(new ProviderAssetOccupancyModel(asset.Id, 1, DateTime.Today,
            TimeSpan.Zero, TimeSpan.FromHours(2)));

        // Assert
        result.ShouldBeFalse();
    }
}