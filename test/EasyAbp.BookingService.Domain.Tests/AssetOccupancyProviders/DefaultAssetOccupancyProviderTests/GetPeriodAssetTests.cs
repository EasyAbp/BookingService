using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetOccupancyCounts;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancyProviders.DefaultAssetOccupancyProviderTests;

public class GetPeriodAssetTests : DefaultAssetOccupancyProviderTestBase
{
    [Fact]
    public async Task Baseline_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        var period = defaultPeriodScheme.Periods[0];
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));
        var currentDateTime = new DateTime(2022, 6, 17);
        var targetDate = new DateTime(2022, 6, 18);

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);
        var actualPeriod = actualPeriods[0];
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(asset.Volume);
        actualPeriod.EndingTime.ShouldBe(period.GetEndingTime());
        actualPeriod.PeriodId.ShouldBe(period.Id);
        actualPeriod.StartingTime.ShouldBe(period.StartingTime);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
    }

    [Theory]
    [InlineData(nameof(AssetPeriodScheme))]
    [InlineData(nameof(Asset))]
    [InlineData(nameof(AssetCategory))]
    [InlineData(null)]
    public async Task PeriodScheme_FallbackChain_Test(string testSubject)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));

        var anotherPeriodScheme = await CreatePeriodScheme();
        anotherPeriodScheme.Periods.Clear();
        var anotherPeriod = await PeriodSchemeManager.CreatePeriodAsync(TimeSpan.FromHours(1), TimeSpan.FromHours(2));
        anotherPeriodScheme.Periods.Add(anotherPeriod);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(anotherPeriodScheme));


        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        switch (testSubject)
        {
            case nameof(AssetPeriodScheme):
                await WithUnitOfWorkAsync(() => AssetPeriodSchemeRepository.InsertAsync(new AssetPeriodScheme(
                    new AssetPeriodSchemeKey
                    {
                        Date = targetDate,
                        AssetId = asset.Id
                    }, default, anotherPeriodScheme.Id)));
                goto case nameof(Asset);
            case nameof(Asset):
                await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
                    anotherPeriodScheme.Id, asset.DefaultPeriodUsable, asset.Volume, asset.Priority,
                    asset.TimeInAdvance, asset.Disabled);
                goto case nameof(AssetCategory);
            case nameof(AssetCategory):
                await AssetCategoryManager.UpdateAsync(category, category.ParentId, category.DisplayName,
                    anotherPeriodScheme.Id, category.DefaultPeriodUsable, category.TimeInAdvance, category.Disabled);
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testSubject), testSubject, "Invalid test subject name");
        }

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        var actualPeriod = actualPeriods[0];
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(asset.Volume);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        switch (testSubject)
        {
            case nameof(AssetPeriodScheme):
            case nameof(Asset):
            case nameof(AssetCategory):
                actualPeriods.Count.ShouldBe(anotherPeriodScheme.Periods.Count);
                actualPeriod.EndingTime.ShouldBe(anotherPeriod.GetEndingTime());
                actualPeriod.PeriodId.ShouldBe(anotherPeriod.Id);
                actualPeriod.StartingTime.ShouldBe(anotherPeriod.StartingTime);
                actualPeriod.PeriodSchemeId.ShouldBe(anotherPeriodScheme.Id);
                break;
            case null:
                actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);
                actualPeriod.EndingTime.ShouldBe(defaultPeriodScheme.Periods[0].GetEndingTime());
                actualPeriod.PeriodId.ShouldBe(defaultPeriodScheme.Periods[0].Id);
                actualPeriod.StartingTime.ShouldBe(defaultPeriodScheme.Periods[0].StartingTime);
                actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testSubject), testSubject, "Invalid test subject name");
        }
    }

    [Theory]
    [InlineData(nameof(Asset))]
    [InlineData(nameof(AssetCategory))]
    [InlineData(null)]
    public async Task PeriodUsable_FallbackChain_Test(string testSubject)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        var period = defaultPeriodScheme.Periods[0];
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        switch (testSubject)
        {
            case nameof(Asset):
                await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
                    asset.PeriodSchemeId, PeriodUsable.Reject, asset.Volume, asset.Priority,
                    asset.TimeInAdvance, asset.Disabled);
                goto case nameof(AssetCategory);
            case nameof(AssetCategory):
                await AssetCategoryManager.UpdateAsync(category, category.ParentId, category.DisplayName,
                    category.PeriodSchemeId, PeriodUsable.Reject, category.TimeInAdvance, category.Disabled);
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testSubject), testSubject, "Invalid test subject name");
        }

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);
        var actualPeriod = actualPeriods[0];
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(testSubject is null ? 1 : 0);
        actualPeriod.EndingTime.ShouldBe(period.GetEndingTime());
        actualPeriod.PeriodId.ShouldBe(period.Id);
        actualPeriod.StartingTime.ShouldBe(period.StartingTime);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
    }

    [Fact]
    public async Task PeriodUsable_ShouldThrow_AssetDefinitionNotExistsException_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));
        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);
        var options = GetRequiredService<IOptions<BookingServiceOptions>>().Value;
        options.AssetDefinitionConfigurations.Clear();
        options.AssetDefinitionConfigurations.Add(AnotherAssetDefinition);

        // Act & Assert
        await Should.ThrowAsync<AssetDefinitionNotExistsException>(() =>
            AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime));
    }

    [Theory]
    [InlineData(nameof(Asset))]
    [InlineData(nameof(AssetCategory))]
    [InlineData(null)]
    public async Task TimeInAdvance_FallbackChain_Test(string testSubject)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        var period = defaultPeriodScheme.Periods[0];
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));

        var newTimeInAdvance = AnotherAssetDefinition.TimeInAdvance;
        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6,
            currentDateTime.Day + AnotherAssetDefinition.TimeInAdvance.MaxDaysInAdvance + 1);


        switch (testSubject)
        {
            case nameof(Asset):
                await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
                    asset.PeriodSchemeId, asset.DefaultPeriodUsable, asset.Volume, asset.Priority,
                    newTimeInAdvance, asset.Disabled);
                goto case nameof(AssetCategory);
            case nameof(AssetCategory):
                await AssetCategoryManager.UpdateAsync(category, category.ParentId, category.DisplayName,
                    category.PeriodSchemeId, asset.DefaultPeriodUsable, newTimeInAdvance, category.Disabled);
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testSubject), testSubject, "Invalid test subject name");
        }

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);
        var actualPeriod = actualPeriods[0];
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(testSubject is null ? 1 : 0);
        actualPeriod.EndingTime.ShouldBe(period.GetEndingTime());
        actualPeriod.PeriodId.ShouldBe(period.Id);
        actualPeriod.StartingTime.ShouldBe(period.StartingTime);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
    }

    [Fact]
    public async Task TimeInAdvance_ShouldThrow_AssetDefinitionNotExistsException_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, PeriodUsable.Reject, asset.Volume, asset.Priority,
            asset.TimeInAdvance, asset.Disabled);
        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);
        var options = GetRequiredService<IOptions<BookingServiceOptions>>().Value;
        options.AssetDefinitionConfigurations.Clear();
        options.AssetDefinitionConfigurations.Add(AnotherAssetDefinition);

        // Act & Assert
        await Should.ThrowAsync<AssetDefinitionNotExistsException>(() =>
            AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime));
    }

    [Theory]
    [InlineData(nameof(Asset))]
    [InlineData(nameof(AssetCategory))]
    public async Task Disabled_Test(string testSubject)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        var period = defaultPeriodScheme.Periods[0];
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));

        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        switch (testSubject)
        {
            case nameof(Asset):
                await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
                    asset.PeriodSchemeId, asset.DefaultPeriodUsable, asset.Volume, asset.Priority,
                    asset.TimeInAdvance, true);
                break;
            case nameof(AssetCategory):
                await AssetCategoryManager.UpdateAsync(category, category.ParentId, category.DisplayName,
                    category.PeriodSchemeId, category.DefaultPeriodUsable, category.TimeInAdvance, true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testSubject), testSubject, "Invalid test subject name");
        }

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);
        var actualPeriod = actualPeriods[0];
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(0);
        actualPeriod.EndingTime.ShouldBe(period.GetEndingTime());
        actualPeriod.PeriodId.ShouldBe(period.Id);
        actualPeriod.StartingTime.ShouldBe(period.StartingTime);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
    }

    [Fact]
    public async Task AssetSchedule_PeriodUsable_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        var period = defaultPeriodScheme.Periods[0];
        var anotherPeriod = await PeriodSchemeManager.CreatePeriodAsync(TimeSpan.Zero, TimeSpan.FromHours(3));
        defaultPeriodScheme.Periods.Add(anotherPeriod);
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));
        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);
        var assetSchedule = await AssetScheduleManager.CreateAsync(targetDate,
            asset.Id,
            defaultPeriodScheme.Id,
            anotherPeriod.Id,
            PeriodUsable.Reject,
            default);
        await WithUnitOfWorkAsync(() => AssetScheduleRepository.InsertAsync(assetSchedule));

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);

        var actualPeriod = actualPeriods.FirstOrDefault(x => x.PeriodId == period.Id);
        actualPeriod.ShouldNotBeNull();
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(asset.Volume);
        actualPeriod.EndingTime.ShouldBe(period.GetEndingTime());
        actualPeriod.StartingTime.ShouldBe(period.StartingTime);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);

        var actualAnotherPeriod = actualPeriods.FirstOrDefault(x => x.PeriodId == anotherPeriod.Id);
        actualAnotherPeriod.ShouldNotBeNull();
        actualAnotherPeriod.Date.ShouldBe(targetDate);
        actualAnotherPeriod.AvailableVolume.ShouldBe(0);
        actualAnotherPeriod.EndingTime.ShouldBe(anotherPeriod.GetEndingTime());
        actualAnotherPeriod.StartingTime.ShouldBe(anotherPeriod.StartingTime);
        actualAnotherPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualAnotherPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
    }

    [Fact]
    public async Task AssetSchedule_TimeInAdvance_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        var period = defaultPeriodScheme.Periods[0];
        var anotherPeriod = await PeriodSchemeManager.CreatePeriodAsync(TimeSpan.Zero, TimeSpan.FromHours(3));
        defaultPeriodScheme.Periods.Add(anotherPeriod);
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));

        var newTimeInAdvance = AnotherAssetDefinition.TimeInAdvance;
        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6,
            currentDateTime.Day + AnotherAssetDefinition.TimeInAdvance.MaxDaysInAdvance + 1);

        var assetSchedule = await AssetScheduleManager.CreateAsync(targetDate,
            asset.Id,
            defaultPeriodScheme.Id,
            anotherPeriod.Id,
            AssetDefinition.DefaultPeriodUsable,
            newTimeInAdvance);
        await WithUnitOfWorkAsync(() => AssetScheduleRepository.InsertAsync(assetSchedule));

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);

        var actualPeriod = actualPeriods.FirstOrDefault(x => x.PeriodId == period.Id);
        actualPeriod.ShouldNotBeNull();
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(asset.Volume);
        actualPeriod.EndingTime.ShouldBe(period.GetEndingTime());
        actualPeriod.StartingTime.ShouldBe(period.StartingTime);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);

        var actualAnotherPeriod = actualPeriods.FirstOrDefault(x => x.PeriodId == anotherPeriod.Id);
        actualAnotherPeriod.ShouldNotBeNull();
        actualAnotherPeriod.Date.ShouldBe(targetDate);
        actualAnotherPeriod.AvailableVolume.ShouldBe(0);
        actualAnotherPeriod.EndingTime.ShouldBe(anotherPeriod.GetEndingTime());
        actualAnotherPeriod.StartingTime.ShouldBe(anotherPeriod.StartingTime);
        actualAnotherPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualAnotherPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
    }

    [Theory]
    [InlineData(10, 5)]
    [InlineData(5, 5)]
    [InlineData(5, 6)]
    public async Task Occupancies_Volume_Test(int initialVolume, int occupancyVolume)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, asset.DefaultPeriodUsable, initialVolume, asset.Priority,
            asset.TimeInAdvance, asset.Disabled);
        var defaultPeriodScheme = await CreatePeriodScheme();
        var period = defaultPeriodScheme.Periods[0];
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));
        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        await WithUnitOfWorkAsync(() => AssetOccupancyCountRepository.InsertAsync(
            new AssetOccupancyCount(default, asset.Id, asset.Name, targetDate, period.StartingTime,
                period.Duration, occupancyVolume)));

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);
        var actualPeriod = actualPeriods[0];
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(Math.Max(0, initialVolume - occupancyVolume));
        actualPeriod.EndingTime.ShouldBe(period.GetEndingTime());
        actualPeriod.PeriodId.ShouldBe(period.Id);
        actualPeriod.StartingTime.ShouldBe(period.StartingTime);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
    }

    [Fact]
    public async Task Occupancies_Intersection_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        var defaultPeriodScheme = await CreatePeriodScheme();
        var period = defaultPeriodScheme.Periods[0];
        var anotherPeriod = await PeriodSchemeManager.CreatePeriodAsync(period.GetEndingTime(), TimeSpan.FromHours(3));
        defaultPeriodScheme.Periods.Add(anotherPeriod);
        await PeriodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => PeriodSchemeRepository.InsertAsync(defaultPeriodScheme));
        var currentDateTime = new DateTime(2022, 6, 17);
        Clock.Now.Returns(currentDateTime);
        var targetDate = new DateTime(2022, 6, 18);

        await WithUnitOfWorkAsync(() => AssetOccupancyCountRepository.InsertAsync(
            new AssetOccupancyCount(default, asset.Id, asset.Name, targetDate, anotherPeriod.StartingTime,
                anotherPeriod.Duration, asset.Volume)));

        // Act
        var actualPeriods = await AssetOccupancyProvider.GetPeriodsAsync(asset, category, targetDate, currentDateTime);

        // Assert
        actualPeriods.ShouldNotBeEmpty();
        actualPeriods.Count.ShouldBe(defaultPeriodScheme.Periods.Count);

        var actualPeriod = actualPeriods.FirstOrDefault(x => x.PeriodId == period.Id);
        actualPeriod.ShouldNotBeNull();
        actualPeriod.Date.ShouldBe(targetDate);
        actualPeriod.AvailableVolume.ShouldBe(asset.Volume);
        actualPeriod.EndingTime.ShouldBe(period.GetEndingTime());
        actualPeriod.StartingTime.ShouldBe(period.StartingTime);
        actualPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);

        var actualAnotherPeriod = actualPeriods.FirstOrDefault(x => x.PeriodId == anotherPeriod.Id);
        actualAnotherPeriod.ShouldNotBeNull();
        actualAnotherPeriod.Date.ShouldBe(targetDate);
        actualAnotherPeriod.AvailableVolume.ShouldBe(0);
        actualAnotherPeriod.EndingTime.ShouldBe(anotherPeriod.GetEndingTime());
        actualAnotherPeriod.StartingTime.ShouldBe(anotherPeriod.StartingTime);
        actualAnotherPeriod.TotalVolume.ShouldBe(asset.Volume);
        actualAnotherPeriod.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
    }
}