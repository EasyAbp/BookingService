using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancyProviders.DefaultAssetOccupancyProviderTests;
using EasyAbp.BookingService.Assets;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class DefaultAssetInCategorySelectorTests : DefaultAssetOccupancyProviderTestBase
{
    [Fact]
    public async Task Select_Priority_Test()
    {
        // Arrange
        var defaultSelector = ServiceProvider.GetRequiredService<DefaultAssetInCategorySelector>();
        var assetCategory = await CreateAssetCategoryAsync();
        var assets = new List<Asset>();
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                assets.Add(await AssetManager.CreateAsync(nameof(Asset),
                    AssetDefinition.Name,
                    assetCategory,
                    default,
                    default,
                    1,
                    i,
                    default,
                    false));
            }
        }

        assets = RandomHelper.GenerateRandomizedList(assets);

        // Act
        var actual = await defaultSelector.SortAsync(assets);

        // Assert
        for (var i = 1; i < actual.Count; i++)
        {
            actual[i].Priority.ShouldBeLessThanOrEqualTo(actual[i - 1].Priority);
        }
    }

    [Fact]
    public async Task Select_Randomized_Test()
    {
        // Arrange
        var defaultSelector = ServiceProvider.GetRequiredService<DefaultAssetInCategorySelector>();
        var assetCategory = await CreateAssetCategoryAsync();
        var assets = new List<Asset>();
        // more elements, less chance of the same randomization result
        for (var i = 0; i < 100; i++)
        {
            assets.Add(await AssetManager.CreateAsync(nameof(Asset),
                AssetDefinition.Name,
                assetCategory,
                default,
                default,
                1,
                0,
                default,
                false));
        }

        assets = RandomHelper.GenerateRandomizedList(assets);

        // Act
        var actual = await defaultSelector.SortAsync(assets);

        // Assert
        actual.SequenceEqual(assets).ShouldBeFalse();
    }
}