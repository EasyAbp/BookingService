using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Guids;
using Xunit;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeManagerTests : BookingServiceOrleansDomainTestBase
{
    private static readonly AssetDefinition AssetDefinition = new(nameof(AssetDefinition),
        default,
        new TimeInAdvance
        {
            MaxDaysInAdvance = 5
        });

    private readonly PeriodSchemeManager _periodSchemeManager;
    private readonly IPeriodSchemeRepository _periodSchemeRepository;
    private readonly AssetCategoryManager _assetCategoryManager;
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly AssetManager _assetManager;
    private readonly IAssetRepository _assetRepository;
    private readonly AssetScheduleManager _assetScheduleManager;
    private readonly IAssetScheduleRepository _assetScheduleRepository;
    private readonly IAssetPeriodSchemeRepository _assetPeriodSchemeRepository;
    private readonly IGuidGenerator _guidGenerator;

    public PeriodSchemeManagerTests()
    {
        _guidGenerator = GetRequiredService<IGuidGenerator>();
        _periodSchemeManager = GetRequiredService<PeriodSchemeManager>();
        _assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        _assetManager = GetRequiredService<AssetManager>();
        _assetScheduleManager = GetRequiredService<AssetScheduleManager>();
        _assetCategoryRepository = GetRequiredService<IAssetCategoryRepository>();
        _periodSchemeRepository = GetRequiredService<IPeriodSchemeRepository>();
        _assetRepository = GetRequiredService<IAssetRepository>();
        _assetScheduleRepository = GetRequiredService<IAssetScheduleRepository>();
        _assetPeriodSchemeRepository = GetRequiredService<IAssetPeriodSchemeRepository>();
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
    public async Task Create_Update_Test()
    {
        // Arrange
        const string name = nameof(Create_Update_Test);
        var period =
            await _periodSchemeManager.CreatePeriodAsync(TimeSpan.Zero, TimeSpan.FromHours(12));
        var periodScheme = await _periodSchemeManager.CreateAsync(name, new List<Period> { period });
        var newStartingTime = period.StartingTime + TimeSpan.FromHours(1);
        var newDuration = period.Duration + TimeSpan.FromHours(1);

        // Assert
        var actual =
            await WithUnitOfWorkAsync(() =>
                _periodSchemeManager.UpdatePeriodAsync(periodScheme, period.Id, newStartingTime, newDuration));

        // Assert
        actual.StartingTime.ShouldBe(newStartingTime);
        actual.Duration.ShouldBe(newDuration);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UnsetDefault_Test(bool initial)
    {
        // Arrange
        const string name = nameof(UnsetDefault_Test);
        var periodScheme = await _periodSchemeManager.CreateAsync(name, new List<Period>());
        if (initial)
        {
            await _periodSchemeManager.SetAsDefaultAsync(periodScheme);
        }

        // Assert
        await _periodSchemeManager.UnsetDefaultAsync(periodScheme);

        // Assert
        periodScheme.IsDefault.ShouldBeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetAsDefault_Test(bool initial)
    {
        // Arrange
        const string name = nameof(SetAsDefault_Test);
        var periodScheme = await _periodSchemeManager.CreateAsync(name, new List<Period>());
        if (initial)
        {
            await _periodSchemeManager.SetAsDefaultAsync(periodScheme);
        }

        // Assert
        await _periodSchemeManager.SetAsDefaultAsync(periodScheme);

        // Assert
        periodScheme.IsDefault.ShouldBeTrue();
    }

    [Fact]
    public async Task SetAsDefault_ShouldThrow_DefaultPeriodSchemeAlreadyExistsException_Test()
    {
        // Arrange
        const string name = nameof(SetAsDefault_ShouldThrow_DefaultPeriodSchemeAlreadyExistsException_Test);
        var periodScheme = await _periodSchemeManager.CreateAsync(name, new List<Period>());
        await _periodSchemeManager.SetAsDefaultAsync(periodScheme);
        await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(periodScheme));
        const string anotherPeriodSchemeName = nameof(anotherPeriodSchemeName);
        var anotherPeriodScheme = await _periodSchemeManager.CreateAsync(anotherPeriodSchemeName, new List<Period>());

        //Act & Assert
        await Should.ThrowAsync<DefaultPeriodSchemeAlreadyExistsException>(async () =>
        {
            await _periodSchemeManager.SetAsDefaultAsync(anotherPeriodScheme);
        });
    }

    [Theory]
    [InlineData(nameof(AssetCategory))]
    [InlineData(nameof(Asset))]
    [InlineData(nameof(AssetSchedule))]
    [InlineData(nameof(AssetPeriodScheme))]
    [InlineData(null)]
    public async Task IsPeriodSchemeInUse_Test(string testSubject)
    {
        // Arrange
        var periodScheme = await _periodSchemeManager.CreateAsync(nameof(PeriodScheme), new List<Period>());
        periodScheme = await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(periodScheme));

        switch (testSubject)
        {
            case nameof(AssetCategory):
                await CreateAssetCategoryAsync(periodScheme.Id);
                break;
            case nameof(Asset):
                await CreateAssetAsync(await CreateAssetCategoryAsync(default), periodScheme.Id);
                break;
            case nameof(AssetSchedule):
                var asset = await CreateAssetAsync(await CreateAssetCategoryAsync(default), default);
                await CreateAssetScheduleAsync(asset, periodScheme.Id, default);
                break;
            case nameof(AssetPeriodScheme):
                await WithUnitOfWorkAsync(() => _assetPeriodSchemeRepository.InsertAsync(new AssetPeriodScheme(
                    new AssetPeriodSchemeKey
                    {
                        Date = new DateTime(2022, 6, 1),
                        AssetId = _guidGenerator.Create()
                    }, default, periodScheme.Id)));
                break;
        }

        // Act
        var result = await WithUnitOfWorkAsync(() => _periodSchemeManager.IsPeriodSchemeInUseAsync(periodScheme));

        // Assert
        result.ShouldBe(!testSubject.IsNullOrWhiteSpace());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdatePeriod_ShouldThrow_CannotUpdatePeriodInUseException_Async(bool isInUse)
    {
        // Arrange
        var period = await _periodSchemeManager.CreatePeriodAsync(TimeSpan.Zero, TimeSpan.FromHours(1));
        var periodScheme = await _periodSchemeManager.CreateAsync(nameof(PeriodScheme),
            new List<Period> { period });
        periodScheme = await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(periodScheme));

        var asset = await CreateAssetAsync(await CreateAssetCategoryAsync(default), default);
        await CreateAssetScheduleAsync(asset,
            isInUse ? periodScheme.Id : default,
            isInUse ? period.Id : default);

        // Act & Assert
        if (isInUse)
        {
            await Should.ThrowAsync<CannotUpdatePeriodInUseException>(() => WithUnitOfWorkAsync(() =>
                _periodSchemeManager.UpdatePeriodAsync(periodScheme,
                    period.Id, TimeSpan.Zero, TimeSpan.FromHours(2))));
        }
        else
        {
            await Should.NotThrowAsync(() => WithUnitOfWorkAsync(() =>
                _periodSchemeManager.UpdatePeriodAsync(periodScheme,
                    period.Id, TimeSpan.Zero, TimeSpan.FromHours(2))));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeletePeriod_ShouldThrow_CannotDeletePeriodInUseException_Async(bool isInUse)
    {
        // Arrange
        var period = await _periodSchemeManager.CreatePeriodAsync(TimeSpan.Zero, TimeSpan.FromHours(1));
        var periodScheme = await _periodSchemeManager.CreateAsync(nameof(PeriodScheme),
            new List<Period> { period });
        periodScheme = await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(periodScheme));

        var asset = await CreateAssetAsync(await CreateAssetCategoryAsync(default), default);
        await CreateAssetScheduleAsync(asset,
            isInUse ? periodScheme.Id : default,
            isInUse ? period.Id : default);

        // Act & Assert
        if (isInUse)
        {
            await Should.ThrowAsync<CannotDeletePeriodInUseException>(() => WithUnitOfWorkAsync(() =>
                _periodSchemeManager.DeletePeriodAsync(periodScheme, period.Id)));
        }
        else
        {
            await Should.NotThrowAsync(() => WithUnitOfWorkAsync(() =>
                _periodSchemeManager.DeletePeriodAsync(periodScheme, period.Id)));
        }
    }

    private async Task CreateAssetScheduleAsync(Asset asset, Guid? periodSchemeId, Guid? periodId)
    {
        var assetSchedule = await _assetScheduleManager.CreateAsync(new DateTime(2022, 6, 21),
            asset.Id,
            periodSchemeId ?? _guidGenerator.Create(),
            periodId ?? _guidGenerator.Create(),
            default,
            default);
        await WithUnitOfWorkAsync(() => _assetScheduleRepository.InsertAsync(assetSchedule));
    }

    private async Task<Asset> CreateAssetAsync(AssetCategory assetCategory, Guid? periodSchemeId)
    {
        var asset = await _assetManager.CreateAsync(nameof(Asset),
            AssetDefinition.Name,
            assetCategory,
            periodSchemeId,
            default,
            1,
            1,
            default,
            default);
        asset = await WithUnitOfWorkAsync(() => _assetRepository.InsertAsync(asset));
        return asset;
    }

    private async Task<AssetCategory> CreateAssetCategoryAsync(Guid? periodSchemeId)
    {
        var assetCategory = await _assetCategoryManager.CreateAsync(default,
            nameof(AssetCategory),
            AssetDefinition.Name,
            periodSchemeId,
            default,
            default,
            default);
        assetCategory = await WithUnitOfWorkAsync(() => _assetCategoryRepository.InsertAsync(assetCategory));
        return assetCategory;
    }
}