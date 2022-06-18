using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancyCounts;
using EasyAbp.BookingService.Assets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Xunit;
using Xunit.Sdk;

namespace EasyAbp.BookingService.AssetOccupancyProviders.DefaultAssetOccupancyProviderTests;

public class OccupyTests : DefaultAssetOccupancyProviderTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        services.Replace(ServiceDescriptor.Transient(s => Substitute.ForPartsOf<DefaultAssetOccupancyProvider>(
            s.GetRequiredService<IAssetOccupancyCountRepository>(),
            s.GetRequiredService<IUnitOfWorkManager>(),
            s
        )));
    }

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
    public async Task Asset_CanOccupy_Test(int initialVolume, int occupiedVolume, int occupyingVolume,
        bool canOccupy)
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
            new OccupyAssetInfoModel(asset.Id, occupyingVolume, targetDate, period.StartingTime, period.Duration));

        // Assert
        actualCanOccupy.ShouldBe(canOccupy);
    }

    [Theory]
    [InlineData(10, 5, 1)]
    [InlineData(10, 0, 1)]
    [InlineData(10, 0, 10)]
    [InlineData(10, 9, 1)]
    [InlineData(10, 10, 0)]
    [InlineData(5, 10, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(0, 0, 0)]
    public async Task Asset_Occupy_Baseline_Test(int initialVolume, int occupiedVolume, int occupyingVolume)
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
        var (model, assetOccupancy) = await AssetOccupancyProvider.OccupyAsync(
            new OccupyAssetInfoModel(asset.Id, occupyingVolume, targetDate, period.StartingTime, period.Duration),
            default);

        // Assert
        model.ShouldNotBeNull();
        model.AssetId.ShouldBe(asset.Id);
        model.Date.ShouldBe(targetDate);
        model.Duration.ShouldBe(period.Duration);
        model.Volume.ShouldBe(occupyingVolume + occupiedVolume);
        model.StartingTime.ShouldBe(period.StartingTime);
        assetOccupancy.ShouldNotBeNull();
        assetOccupancy.Asset.ShouldBe($"{category.DisplayName}-{asset.Name}");
        assetOccupancy.Date.ShouldBe(targetDate);
        assetOccupancy.Duration.ShouldBe(period.Duration);
        assetOccupancy.Volume.ShouldBe(occupyingVolume);
        assetOccupancy.AssetId.ShouldBe(asset.Id);
        assetOccupancy.StartingTime.ShouldBe(period.StartingTime);
        assetOccupancy.AssetDefinitionName.ShouldBe(AssetDefinition.Name);
    }

    [Fact]
    public async Task Asset_Occupy_GetOccupierName_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, asset.DefaultPeriodUsable, 1, asset.Priority,
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
        });

        var userId = GetRequiredService<IGuidGenerator>().Create();
        const string userName = nameof(userName);
        var userData = Substitute.For<IUserData>();
        userData.UserName.Returns(userName);
        ExternalUserLookupServiceProvider.FindByIdAsync(userId).Returns(Task.FromResult(userData));

        // Act
        var (_, assetOccupancy) = await AssetOccupancyProvider.OccupyAsync(
            new OccupyAssetInfoModel(asset.Id, 1, targetDate, period.StartingTime, period.Duration),
            userId);

        // Assert
        assetOccupancy.OccupierUserId.ShouldBe(userId);
        assetOccupancy.OccupierName.ShouldBe(userName);
    }

    [Fact]
    public async Task Asset_Occupy_RollBackOccupancy_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, asset.DefaultPeriodUsable, 1, asset.Priority,
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
        });

        var userId = GetRequiredService<IGuidGenerator>().Create();
        ExternalUserLookupServiceProvider.FindByIdAsync(Arg.Any<Guid>())
            .ThrowsForAnyArgs(new BusinessException());

        // Act
        await Should.ThrowAsync<BusinessException>(() => AssetOccupancyProvider.OccupyAsync(
            new OccupyAssetInfoModel(asset.Id, 1, targetDate, period.StartingTime, period.Duration),
            userId));
        var assetOccupancyCounts = await AssetOccupancyCountRepository.GetListAsync();

        // Assert
        assetOccupancyCounts.All(x => x.Volume == 0).ShouldBeTrue();
    }

    [Theory]
    [InlineData(nameof(Asset))]
    [InlineData(nameof(AssetCategory))]
    public async Task Asset_Occupy_ShouldThrow_DisabledAssetOrCategoryException_Test(string testSubject)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync(testSubject == nameof(AssetCategory));
        var asset = await CreateAssetAsync(category, testSubject == nameof(Asset));
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, asset.DefaultPeriodUsable, 1, asset.Priority,
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
        });

        // Act & Assert
        await Should.ThrowAsync<DisabledAssetOrCategoryException>(() => AssetOccupancyProvider.OccupyAsync(
            new OccupyAssetInfoModel(asset.Id, 1, targetDate, period.StartingTime, period.Duration),
            default));
    }

    [Theory]
    [InlineData(10, 0, 11)]
    [InlineData(10, 9, 2)]
    [InlineData(10, 10, 1)]
    [InlineData(5, 10, 2)]
    [InlineData(0, 1, 1)]
    public async Task Asset_Occupy_ShouldThrow_InsufficientAssetVolumeException_Test(int initialVolume,
        int occupiedVolume, int occupyingVolume)
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

        // Act & Assert
        await Should.ThrowAsync<InsufficientAssetVolumeException>(() => AssetOccupancyProvider.OccupyAsync(
            new OccupyAssetInfoModel(asset.Id, occupyingVolume, targetDate, period.StartingTime, period.Duration),
            default));
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
    public async Task Category_CanOccupy_Baseline_Test(int initialVolume1, int occupiedVolume1,
        int initialVolume2, int occupiedVolume2,
        int occupyingVolume, bool canOccupy)
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
            new OccupyAssetByCategoryInfoModel(category.Id, occupyingVolume, targetDate, period.StartingTime,
                period.Duration));

        // Assert
        actualCanOccupy.ShouldBe(canOccupy);
    }


    [Theory]
    [InlineData(10, 5, 10, 5, 1)]
    [InlineData(10, 10, 10, 5, 1)]
    [InlineData(10, 5, 10, 10, 1)]
    [InlineData(10, 10, 10, 10, 0)]
    [InlineData(10, 10, 0, 0, 0)]
    [InlineData(0, 0, 10, 10, 0)]
    [InlineData(5, 10, 0, 0, 0)]
    [InlineData(0, 0, 5, 10, 0)]
    public async Task Category_Occupy_Baseline_Test(int initialVolume1, int occupiedVolume1,
        int initialVolume2, int occupiedVolume2,
        int occupyingVolume)
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
        var (model, assetOccupancy) = await AssetOccupancyProvider.OccupyByCategoryAsync(
            new OccupyAssetByCategoryInfoModel(category.Id, occupyingVolume, targetDate, period.StartingTime,
                period.Duration), default);

        // Assert
        var occupiedVolume = model.AssetId == asset1.Id ? occupiedVolume1 : occupiedVolume2;
        var selectedAsset = model.AssetId == asset1.Id ? asset1 : asset2;

        model.ShouldNotBeNull();
        model.Date.ShouldBe(targetDate);
        model.Duration.ShouldBe(period.Duration);
        model.Volume.ShouldBe(occupyingVolume + occupiedVolume);
        model.StartingTime.ShouldBe(period.StartingTime);

        assetOccupancy.ShouldNotBeNull();
        assetOccupancy.Asset.ShouldBe($"{category.DisplayName}-{selectedAsset.Name}");
        assetOccupancy.Date.ShouldBe(targetDate);
        assetOccupancy.Duration.ShouldBe(period.Duration);
        assetOccupancy.Volume.ShouldBe(occupyingVolume);
        assetOccupancy.AssetId.ShouldBe(selectedAsset.Id);
        assetOccupancy.StartingTime.ShouldBe(period.StartingTime);
        assetOccupancy.AssetDefinitionName.ShouldBe(AssetDefinition.Name);
    }

    [Fact]
    public async Task Category_Occupy_GetOccupierName_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, asset.DefaultPeriodUsable, 1, asset.Priority,
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
        });

        var userId = GetRequiredService<IGuidGenerator>().Create();
        const string userName = nameof(userName);
        var userData = Substitute.For<IUserData>();
        userData.UserName.Returns(userName);
        ExternalUserLookupServiceProvider.FindByIdAsync(userId).Returns(Task.FromResult(userData));

        // Act
        var (_, assetOccupancy) = await AssetOccupancyProvider.OccupyByCategoryAsync(
            new OccupyAssetByCategoryInfoModel(category.Id, 1, targetDate, period.StartingTime, period.Duration),
            userId);

        // Assert
        assetOccupancy.OccupierUserId.ShouldBe(userId);
        assetOccupancy.OccupierName.ShouldBe(userName);
    }

    [Fact]
    public async Task Category_Occupy_RollBackOccupancy_Test()
    {
        // Arrange
        var category = await CreateAssetCategoryAsync();
        var asset = await CreateAssetAsync(category);
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, asset.DefaultPeriodUsable, 1, asset.Priority,
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
        });

        var userId = GetRequiredService<IGuidGenerator>().Create();
        ExternalUserLookupServiceProvider.FindByIdAsync(Arg.Any<Guid>())
            .ThrowsForAnyArgs(new BusinessException());

        // Act
        await Should.ThrowAsync<BusinessException>(() => AssetOccupancyProvider.OccupyByCategoryAsync(
            new OccupyAssetByCategoryInfoModel(category.Id, 1, targetDate, period.StartingTime, period.Duration),
            userId));
        var assetOccupancyCounts = await AssetOccupancyCountRepository.GetListAsync();

        // Assert
        assetOccupancyCounts.All(x => x.Volume == 0).ShouldBeTrue();
    }

    [Theory]
    [InlineData(nameof(Asset))]
    [InlineData(nameof(AssetCategory))]
    public async Task Category_Occupy_ShouldThrow_DisabledAssetOrCategoryException_Test(string testSubject)
    {
        // Arrange
        var category = await CreateAssetCategoryAsync(testSubject == nameof(AssetCategory));
        var asset = await CreateAssetAsync(category, testSubject == nameof(Asset));
        await AssetManager.UpdateAsync(asset, asset.Name, asset.AssetDefinitionName, category,
            asset.PeriodSchemeId, asset.DefaultPeriodUsable, 1, asset.Priority,
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
        });

        switch (testSubject)
        {
            // Act & Assert
            case nameof(AssetCategory):
                await Should.ThrowAsync<DisabledAssetOrCategoryException>(() =>
                    AssetOccupancyProvider.OccupyByCategoryAsync(
                        new OccupyAssetByCategoryInfoModel(category.Id, 1, targetDate, period.StartingTime,
                            period.Duration),
                        default));
                break;
            case nameof(Asset):
                await Should.ThrowAsync<InsufficientAssetVolumeException>(() =>
                    AssetOccupancyProvider.OccupyByCategoryAsync(
                        new OccupyAssetByCategoryInfoModel(category.Id, 1, targetDate, period.StartingTime,
                            period.Duration),
                        default));
                break;
            default:
                throw new XunitException();
        }
    }

    [Theory]
    [InlineData(10, 0, 11)]
    [InlineData(10, 9, 2)]
    [InlineData(10, 10, 1)]
    [InlineData(5, 10, 2)]
    [InlineData(0, 1, 1)]
    public async Task Category_Occupy_ShouldThrow_InsufficientAssetVolumeException_Test(int initialVolume,
        int occupiedVolume, int occupyingVolume)
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

        // Act & Assert
        await Should.ThrowAsync<InsufficientAssetVolumeException>(() => AssetOccupancyProvider.OccupyByCategoryAsync(
            new OccupyAssetByCategoryInfoModel(category.Id, occupyingVolume, targetDate, period.StartingTime,
                period.Duration),
            default));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Category_Occupy_AssetPriority_Test(bool firstHighPriority)
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
        var (_, occupancy) = await AssetOccupancyProvider.OccupyByCategoryAsync(
            new OccupyAssetByCategoryInfoModel(category.Id, 1, targetDate, period.StartingTime,
                period.Duration), null);

        // Assert
        occupancy.AssetId.ShouldBe(firstHighPriority ? asset1.Id : asset2.Id);
    }
}