using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public class DefaultPeriodSchemeStore : DomainService
{
    private readonly IPeriodSchemeRepository _periodSchemeRepository;
    private readonly IDistributedCache<DefaultPeriodSchemeCacheItem> _distributedCache;

    public DefaultPeriodSchemeStore(IPeriodSchemeRepository periodSchemeRepository,
        IDistributedCache<DefaultPeriodSchemeCacheItem> distributedCache)
    {
        _periodSchemeRepository = periodSchemeRepository;
        _distributedCache = distributedCache;
    }

    public async Task<PeriodScheme> GetAsync()
    {
        var item = await _distributedCache.GetOrAddAsync(DefaultPeriodSchemeCacheItem.Key, DefaultPeriodSchemeFactory);
        return item.Value;
    }

    public async Task ClearAsync()
    {
        await _distributedCache.RemoveAsync(DefaultPeriodSchemeCacheItem.Key, considerUow: true);
    }

    private async Task<DefaultPeriodSchemeCacheItem> DefaultPeriodSchemeFactory()
    {
        var item = new DefaultPeriodSchemeCacheItem
        {
            Value = await _periodSchemeRepository.GetDefaultSchemeAsync()
        };

        return item;
    }
}