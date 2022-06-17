using System;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancyProviders.DefaultAssetOccupancyProviderTests;

public class OccupyTests : DefaultAssetOccupancyProviderTestBase
{
    [Fact]
    public async Task Asset_Baseline_Test()
    {
    }

    [Fact]
    public async Task Asset_GetOccupierName_Test()
    {
    }

    [Fact]
    public async Task Asset_RollBackOccupancy_Test()
    {
    }

    [Fact]
    public async Task Asset_ShouldThrow_DisabledAssetOrCategoryException_Test()
    {
    }

    [Fact]
    public async Task Asset_ShouldThrow_InsufficientAssetVolumeException_Test()
    {
    }

    [Fact]
    public async Task Category_Baseline_Test()
    {
    }

    [Fact]
    public async Task Category_GetOccupierName_Test()
    {
    }

    [Fact]
    public async Task Category_RollBackOccupancy_Test()
    {
    }

    [Fact]
    public async Task Category_ShouldThrow_DisabledAssetOrCategoryException_Test()
    {
    }

    [Fact]
    public async Task Category_ShouldThrow_InsufficientAssetVolumeException_Test()
    {
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Category_AssetPriority_Test(bool firstHighPriority)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();

        var asset1 = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset1, asset1.Name, asset1.AssetDefinitionName, category,
            asset1.PeriodSchemeId, asset1.DefaultPeriodUsable, asset1.Volume,
            asset1.Priority + (firstHighPriority ? 1 : 0),
            asset1.TimeInAdvance, asset1.Disabled);

        var asset2 = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset2, asset2.Name, asset2.AssetDefinitionName, category,
            asset2.PeriodSchemeId, asset2.DefaultPeriodUsable, asset2.Volume,
            asset2.Priority + (firstHighPriority ? 0 : 1),
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
        });

        // Act
        var (model, occupancy) = await AssetOccupancyProvider.OccupyByCategoryAsync(
            new OccupyAssetByCategoryInfoModel(category.Id, 1, targetDate, period.StartingTime,
                period.Duration), null);

        // Assert
        occupancy.AssetId.ShouldBe(firstHighPriority ? asset1.Id : asset2.Id);
    }

    [Fact]
    public async Task BulkOccupy_Baseline_Test()
    {
        //TODO
    }
}