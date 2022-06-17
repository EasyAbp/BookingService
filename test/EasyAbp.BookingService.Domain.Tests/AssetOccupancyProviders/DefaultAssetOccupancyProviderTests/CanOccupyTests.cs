using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancyCounts;
using NSubstitute;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancyProviders.DefaultAssetOccupancyProviderTests;

public class CanOccupyTests : DefaultAssetOccupancyProviderTestBase
{
    [Theory]
    [InlineData(10, 5, 1, true)]
    [InlineData(10, 0, 1, true)]
    [InlineData(10, 0, 10, true)]
    [InlineData(10, 0, 11, false)]
    [InlineData(10, 9, 1, true)]
    [InlineData(10, 9, 2, false)]
    [InlineData(10, 10, 1, false)]
    [InlineData(10, 10, 0, true)]
    [InlineData(5, 10, 2, false)]
    [InlineData(5, 10, 0, true)]
    [InlineData(0, 1, 0, true)]
    [InlineData(0, 1, 1, false)]
    [InlineData(0, 0, 0, true)]
    public async Task Asset_Baseline_Test(int initialVolume, int occupiedVolume, int occupyVolume, bool canOccupy)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, asset.DefaultPeriodUsable, initialVolume, asset.Priority,
            asset.TimeInAdvance, asset.Disabled);
        var periodScheme = await CreatePeriodScheme();
        var period = periodScheme.Periods[0];
        await PeriodSchemeManager.SetAsDefaultAsync(periodScheme);

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        await WithUnitOfWorkAsync(async () =>
        {
            await AssetCategoryRepository.InsertAsync(category);
            await AssetRepository.InsertAsync(asset);
            await PeriodSchemeRepository.InsertAsync(periodScheme);
            if (occupiedVolume > 0)
            {
                await AssetOccupancyCountRepository.InsertAsync(
                    new AssetOccupancyCount(default, asset.Id, asset.Name, targetDate, period.StartingTime,
                        period.Duration, occupiedVolume));
            }
        });


        // Act
        var actualCanOccupy = await AssetOccupancyProvider.CanOccupyAsync(
            new OccupyAssetInfoModel(asset.Id, occupyVolume, targetDate, period.StartingTime, period.Duration));

        // Assert
        actualCanOccupy.ShouldBe(canOccupy);
    }

    [Theory]
    [InlineData(10, 5, 10, 5, 1, true)]
    [InlineData(10, 10, 10, 5, 1, true)]
    [InlineData(10, 5, 10, 10, 1, true)]
    [InlineData(10, 10, 10, 10, 1, false)]
    [InlineData(10, 10, 10, 10, 0, true)]
    [InlineData(10, 10, 0, 0, 0, true)]
    [InlineData(0, 0, 10, 10, 0, true)]
    [InlineData(10, 10, 0, 0, 1, false)]
    [InlineData(0, 0, 10, 10, 1, false)]
    [InlineData(5, 10, 0, 0, 0, true)]
    [InlineData(0, 0, 5, 10, 0, true)]
    [InlineData(5, 10, 0, 0, 1, false)]
    [InlineData(0, 0, 5, 10, 1, false)]
    public async Task Category_Baseline_Test(int initialVolume1, int occupiedVolume1,
        int initialVolume2, int occupiedVolume2,
        int occupyVolume, bool canOccupy)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();

        var asset1 = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset1, asset1.Name, asset1.AssetDefinitionName, category,
            asset1.PeriodSchemeId, asset1.DefaultPeriodUsable, initialVolume1, asset1.Priority,
            asset1.TimeInAdvance, asset1.Disabled);

        var asset2 = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset2, asset2.Name, asset2.AssetDefinitionName, category,
            asset2.PeriodSchemeId, asset2.DefaultPeriodUsable, initialVolume2, asset2.Priority,
            asset2.TimeInAdvance, asset2.Disabled);

        var periodScheme = await CreatePeriodScheme();
        var period = periodScheme.Periods[0];
        await PeriodSchemeManager.SetAsDefaultAsync(periodScheme);

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        await WithUnitOfWorkAsync(async () =>
        {
            await AssetCategoryRepository.InsertAsync(category);
            await AssetRepository.InsertAsync(asset1);
            await AssetRepository.InsertAsync(asset2);
            await PeriodSchemeRepository.InsertAsync(periodScheme);
            if (occupiedVolume1 > 0)
            {
                await AssetOccupancyCountRepository.InsertAsync(
                    new AssetOccupancyCount(default, asset1.Id, asset1.Name, targetDate, period.StartingTime,
                        period.Duration, occupiedVolume1));
            }

            if (occupiedVolume2 > 0)
            {
                await AssetOccupancyCountRepository.InsertAsync(
                    new AssetOccupancyCount(default, asset2.Id, asset2.Name, targetDate, period.StartingTime,
                        period.Duration, occupiedVolume2));
            }
        });

        // Act
        var actualCanOccupy = await AssetOccupancyProvider.CanOccupyByCategoryAsync(
            new OccupyAssetByCategoryInfoModel(category.Id, occupyVolume, targetDate, period.StartingTime,
                period.Duration));

        // Assert
        actualCanOccupy.ShouldBe(canOccupy);
    }

    [Fact]
    public async Task BulkOccupy_Baseline_Test()
    {
        //TODO
    }
}