using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.Domain.Entities.Events;
using Xunit;

namespace EasyAbp.BookingService.PeriodSchemes;

public class DefaultPeriodSchemeProviderCacheCleanerTests : BookingServiceDomainTestBase
{
    private IDefaultPeriodSchemeProvider _defaultPeriodSchemeProvider;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        _defaultPeriodSchemeProvider = Substitute.For<IDefaultPeriodSchemeProvider>();
        services.Replace(ServiceDescriptor.Transient(s => _defaultPeriodSchemeProvider));
    }

    [Fact]
    public async Task HandleEvent_Test()
    {
        //Arrange
        var cleaner = GetRequiredService<DefaultPeriodSchemeProviderCacheCleaner>();

        //Act
        await cleaner.HandleEventAsync(new EntityChangedEventData<PeriodScheme>(default));

        //Assert
        await _defaultPeriodSchemeProvider.Received(1).ClearCacheAsync();
    }
}