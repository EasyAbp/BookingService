using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Caching;
using Xunit;

namespace EasyAbp.BookingService.PeriodSchemes;

public class DefaultPeriodSchemeProviderTests : BookingServiceDomainTestBase
{
    private readonly IPeriodSchemeRepository _periodSchemeRepository;
    private readonly PeriodSchemeManager _periodSchemeManager;
    private readonly IDistributedCache<DefaultPeriodSchemeCacheItem> _distributedCache;
    private readonly IDefaultPeriodSchemeProvider _defaultPeriodSchemeProvider;

    public DefaultPeriodSchemeProviderTests()
    {
        _periodSchemeRepository = GetRequiredService<IPeriodSchemeRepository>();
        _periodSchemeManager = GetRequiredService<PeriodSchemeManager>();
        _distributedCache = GetRequiredService<IDistributedCache<DefaultPeriodSchemeCacheItem>>();
        _defaultPeriodSchemeProvider = GetRequiredService<IDefaultPeriodSchemeProvider>();
    }

    [Fact]
    public async Task Get_FromCache_Test()
    {
        //Arrange
        const string name = nameof(Get_FromCache_Test);
        var defaultPeriodScheme = await _periodSchemeManager.CreateAsync(name, new List<Period>());
        await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(defaultPeriodScheme));
        await _distributedCache.SetAsync(DefaultPeriodSchemeCacheItem.Key, new DefaultPeriodSchemeCacheItem
        {
            PeriodSchemeId = defaultPeriodScheme.Id
        });

        //Act
        var actual = await _defaultPeriodSchemeProvider.GetAsync();

        //Assert
        actual.Id.ShouldBe(defaultPeriodScheme.Id);
        actual.Name.ShouldBe(defaultPeriodScheme.Name);
    }

    [Fact]
    public async Task Get_FromRepository_Test()
    {
        //Arrange
        const string name = nameof(Get_FromRepository_Test);
        var defaultPeriodScheme = await _periodSchemeManager.CreateAsync(name, new List<Period>());
        await _periodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(defaultPeriodScheme));

        //Act
        var actual = await _defaultPeriodSchemeProvider.GetAsync();
        var cacheItem = await _distributedCache.GetAsync(DefaultPeriodSchemeCacheItem.Key);

        //Assert
        cacheItem.ShouldNotBeNull();
        cacheItem.PeriodSchemeId.ShouldBe(defaultPeriodScheme.Id);
        actual.Id.ShouldBe(defaultPeriodScheme.Id);
        actual.Name.ShouldBe(defaultPeriodScheme.Name);
    }

    [Fact]
    public async Task Get_ShouldThrow_DefaultPeriodSchemeNotFoundException_Test()
    {
        //Arrange
        const string name = nameof(Get_ShouldThrow_DefaultPeriodSchemeNotFoundException_Test);
        var defaultPeriodScheme = await _periodSchemeManager.CreateAsync(name, new List<Period>());
        await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(defaultPeriodScheme));

        //Act & Assert
        await Should.ThrowAsync<DefaultPeriodSchemeNotFoundException>(async () =>
        {
            await _defaultPeriodSchemeProvider.GetAsync();
        });
    }

    [Fact]
    public async Task ClearCache_Test()
    {
        //Arrange
        const string name = nameof(Get_FromRepository_Test);
        var defaultPeriodScheme = await _periodSchemeManager.CreateAsync(name, new List<Period>());
        await _periodSchemeManager.SetAsDefaultAsync(defaultPeriodScheme);
        await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(defaultPeriodScheme));
        await _defaultPeriodSchemeProvider.GetAsync();

        //Act
        await _defaultPeriodSchemeProvider.ClearCacheAsync();
        var cacheItem = await _distributedCache.GetAsync(DefaultPeriodSchemeCacheItem.Key);

        //Assert
        cacheItem.ShouldBeNull();
    }
}