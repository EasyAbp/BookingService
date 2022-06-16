using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetSchedules;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Guids;
using Xunit;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategoryManagerTests : BookingServiceDomainTestBase
{
    private readonly AssetCategoryManager _assetCategoryManager;
    private readonly IGuidGenerator _guid;
    private readonly AssetDefinition _assetDefinition;

    public AssetCategoryManagerTests()
    {
        _assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        _guid = GetRequiredService<IGuidGenerator>();
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
        var parentId = _guid.Create();
        const string displayName = nameof(Create_Test);
        var assetDefinitionName = _assetDefinition.Name;
        var periodSchemeId = _guid.Create();
        const PeriodUsable defaultPeriodUsable = PeriodUsable.Accept;
        var timeInAdvance = new TimeInAdvance
        {
            MaxDaysInAdvance = 5
        };
        var disabled = true;

        // Act
        var assetCategory = await _assetCategoryManager.CreateAsync(parentId,
            displayName,
            assetDefinitionName,
            periodSchemeId,
            defaultPeriodUsable,
            timeInAdvance,
            disabled);

        // Assert
        assetCategory.ParentId.ShouldBe(parentId);
        assetCategory.DisplayName.ShouldBe(displayName);
        assetCategory.AssetDefinitionName.ShouldBe(assetDefinitionName);
        assetCategory.PeriodSchemeId.ShouldBe(periodSchemeId);
        assetCategory.DefaultPeriodUsable.ShouldBe(defaultPeriodUsable);
        assetCategory.TimeInAdvance.ShouldNotBeNull();
        assetCategory.TimeInAdvance.MaxDaysInAdvance.ShouldBe(timeInAdvance.MaxDaysInAdvance);
        assetCategory.Disabled.ShouldBe(disabled);
    }

    [Fact]
    public async Task Create_ShouldThrowAssetDefinitionNotExistsException_Test()
    {
        // Arrange
        const string assetDefinitionName = "InvalidAssetDefinitionName";

        // Act & Assert
        await Should.ThrowAsync<AssetDefinitionNotExistsException>(async () =>
        {
            await _assetCategoryManager.CreateAsync(default,
                default,
                assetDefinitionName,
                default,
                default,
                default,
                default);
        });
    }
}