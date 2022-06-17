using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeManagerTests : BookingServiceDomainTestBase
{
    private readonly PeriodSchemeManager _periodSchemeManager;
    private readonly IPeriodSchemeRepository _periodSchemeRepository;

    public PeriodSchemeManagerTests()
    {
        _periodSchemeManager = GetRequiredService<PeriodSchemeManager>();
        _periodSchemeRepository = GetRequiredService<IPeriodSchemeRepository>();
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
            await _periodSchemeManager.UpdatePeriodAsync(periodScheme, period.Id, newStartingTime, newDuration);

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
}