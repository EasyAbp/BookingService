using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.Assets
{
    public class AssetManagerTests : BookingServiceDomainTestBase
    {
        private readonly AssetManager _assetManager;
        private readonly AssetDefinition _assetDefinition;

        public AssetManagerTests()
        {
            _assetManager = GetRequiredService<AssetManager>();
            _assetDefinition = new AssetDefinition(nameof(AssetDefinition),
                default,
                new TimeInAdvance
                {
                    MaxDaysInAdvance = 5
                });
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<BookingServiceOptions>(options =>
            {
                options.AssetDefinitionConfigurations = new List<AssetDefinition>
                {
                    _assetDefinition
                };
            });
        }

        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            const string name = nameof(Create_Test);
            var assetDefinitionName = _assetDefinition.Name;
            var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            var assetCategory =
                await assetCategoryManager.CreateAsync(default,
                    nameof(AssetCategory),
                    _assetDefinition.Name,
                    default,
                    default,
                    default,
                    default);
            const int volume = 10;
            const int priority = 10;

            // Act
            var asset = await _assetManager.CreateAsync(name,
                assetDefinitionName,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default);

            // Assert
            asset.Name.ShouldBe(name);
            asset.AssetDefinitionName.ShouldBe(assetDefinitionName);
            asset.AssetCategoryId.ShouldBe(assetCategory.Id);
            asset.PeriodSchemeId.ShouldBe(default);
            asset.DefaultPeriodUsable.ShouldBe(default);
            asset.Volume.ShouldBe(volume);
            asset.Priority.ShouldBe(priority);
            asset.TimeInAdvance.ShouldBe(default);
            asset.Disabled.ShouldBe(default);
        }

        [Fact]
        public async Task Create_EmptyAssetDefinitionName_Test()
        {
            // Arrange
            const string name = nameof(Create_EmptyAssetDefinitionName_Test);
            var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            var assetCategory =
                await assetCategoryManager.CreateAsync(default,
                    nameof(AssetCategory),
                    _assetDefinition.Name,
                    default,
                    default,
                    default,
                    default);
            const int volume = 10;
            const int priority = 10;

            // Act & Assert
            await Should.ThrowAsync<ArgumentException>(() => _assetManager.CreateAsync(name,
                string.Empty,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default));
        }

        [Fact]
        public async Task Create_ShouldThrow_AssetDefinitionNotExistsException_Test()
        {
            // Arrange
            const string name = nameof(Create_ShouldThrow_AssetDefinitionNotExistsException_Test);
            var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            var assetCategory =
                await assetCategoryManager.CreateAsync(default,
                    nameof(AssetCategory),
                    _assetDefinition.Name,
                    default,
                    default,
                    default,
                    default);
            const int volume = 10;
            const int priority = 10;

            // Act & Assert
            await Should.ThrowAsync<AssetDefinitionNotExistsException>(() => _assetManager.CreateAsync(name,
                "anotherAssetDefinitionName",
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default));
        }

        [Fact]
        public async Task Create_ShouldThrow_AssetDefinitionNameNotMatchException_Test()
        {
            // Arrange
            const string name = nameof(Create_ShouldThrow_AssetDefinitionNameNotMatchException_Test);
            var assetDefinitionName = _assetDefinition.Name;
            var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            var assetCategory =
                await assetCategoryManager.CreateAsync(default,
                    nameof(AssetCategory),
                    "anotherAssetDefinitionName",
                    default,
                    default,
                    default,
                    default);
            const int volume = 10;
            const int priority = 10;

            // Act & Assert
            await Should.ThrowAsync<AssetDefinitionNameNotMatchException>(() => _assetManager.CreateAsync(name,
                assetDefinitionName,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default));
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            const string name = nameof(Update_Test);
            var assetDefinitionName = _assetDefinition.Name;
            var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            var assetCategory =
                await assetCategoryManager.CreateAsync(default,
                    nameof(AssetCategory),
                    _assetDefinition.Name,
                    default,
                    default,
                    default,
                    default);
            const int volume = 10;
            const int priority = 10;

            const string newName = nameof(newName);

            var asset = await _assetManager.CreateAsync(name,
                assetDefinitionName,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default);

            // Act
            await _assetManager.UpdateAsync(asset, newName, assetDefinitionName,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default);

            // Assert
            asset.Name.ShouldBe(newName);
        }

        [Fact]
        public async Task Update_EmptyAssetDefinitionName_Test()
        {
            // Arrange
            const string name = nameof(Update_EmptyAssetDefinitionName_Test);
            var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            var assetDefinitionName = _assetDefinition.Name;
            var assetCategory =
                await assetCategoryManager.CreateAsync(default,
                    nameof(AssetCategory),
                    _assetDefinition.Name,
                    default,
                    default,
                    default,
                    default);
            const int volume = 10;
            const int priority = 10;
            var asset = await _assetManager.CreateAsync(name,
                assetDefinitionName,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default);

            // Act & Assert
            await Should.ThrowAsync<ArgumentException>(() => _assetManager.UpdateAsync(asset,
                name,
                string.Empty,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default));
        }

        [Fact]
        public async Task Update_ShouldThrow_AssetDefinitionNotExistsException_Test()
        {
            // Arrange
            const string name = nameof(Create_Test);
            var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            var assetDefinitionName = _assetDefinition.Name;
            var assetCategory =
                await assetCategoryManager.CreateAsync(default,
                    nameof(AssetCategory),
                    _assetDefinition.Name,
                    default,
                    default,
                    default,
                    default);
            const int volume = 10;
            const int priority = 10;
            var asset = await _assetManager.CreateAsync(name,
                assetDefinitionName,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default);

            // Act & Assert
            await Should.ThrowAsync<AssetDefinitionNotExistsException>(() => _assetManager.UpdateAsync(asset,
                name,
                "anotherAssetDefinitionName",
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default));
        }

        [Fact]
        public async Task Update_ShouldThrow_AssetDefinitionNameNotMatchException_Test()
        {
            // Arrange
            const string name = nameof(Update_ShouldThrow_AssetDefinitionNameNotMatchException_Test);
            var assetDefinitionName = _assetDefinition.Name;
            var assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            var assetCategory =
                await assetCategoryManager.CreateAsync(default,
                    nameof(AssetCategory),
                    "anotherAssetDefinitionName",
                    default,
                    default,
                    default,
                    default);
            const int volume = 10;
            const int priority = 10;
            var asset = await _assetManager.CreateAsync(name,
                assetDefinitionName,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default);

            // Act & Assert
            await Should.ThrowAsync<AssetDefinitionNameNotMatchException>(() => _assetManager.UpdateAsync(asset,
                name,
                assetDefinitionName,
                assetCategory,
                default,
                default,
                volume,
                priority,
                default,
                default));
        }
    }
}