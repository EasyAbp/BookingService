using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.BookingService.PeriodSchemes;

public class DefaultPeriodSchemeStore : ITransientDependency
{
    private readonly IPeriodSchemeRepository _periodSchemeRepository;
    private readonly IDistributedCache<DefaultPeriodSchemeCacheItem> _distributedCache;

    public DefaultPeriodSchemeStore(IPeriodSchemeRepository periodSchemeRepository,
        IDistributedCache<DefaultPeriodSchemeCacheItem> distributedCache)
    {
        _periodSchemeRepository = periodSchemeRepository;
        _distributedCache = distributedCache;
    }

    [ItemCanBeNull]
    public virtual async Task<PeriodScheme> GetAsync()
    {
        var item = await _distributedCache.GetAsync(DefaultPeriodSchemeCacheItem.Key);
        if (item is null)
        {
            var defaultPeriodScheme = await _periodSchemeRepository.FindDefaultSchemeAsync();
            if (defaultPeriodScheme is not null)
            {
                await _distributedCache.SetAsync(DefaultPeriodSchemeCacheItem.Key, new DefaultPeriodSchemeCacheItem
                {
                    PeriodSchemeId = defaultPeriodScheme.Id,
                });
            }

            return defaultPeriodScheme;
        }
        else
        {
            return await _periodSchemeRepository.GetAsync(item.PeriodSchemeId);
        }
    }

    public virtual async Task ClearAsync()
    {
        await _distributedCache.RemoveAsync(DefaultPeriodSchemeCacheItem.Key, considerUow: true);
    }
}