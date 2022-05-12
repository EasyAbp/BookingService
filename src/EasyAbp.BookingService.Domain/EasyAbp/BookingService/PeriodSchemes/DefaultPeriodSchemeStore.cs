using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

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

    public virtual async Task<PeriodScheme> GetAsync()
    {
        var item = await _distributedCache.GetOrAddAsync(DefaultPeriodSchemeCacheItem.Key, DefaultPeriodSchemeFactory);
        return item.Value;
    }

    public virtual async Task ClearAsync()
    {
        await _distributedCache.RemoveAsync(DefaultPeriodSchemeCacheItem.Key, considerUow: true);
    }

    [UnitOfWork]
    protected virtual async Task<DefaultPeriodSchemeCacheItem> DefaultPeriodSchemeFactory()
    {
        var item = new DefaultPeriodSchemeCacheItem
        {
            Value = await _periodSchemeRepository.FindDefaultSchemeAsync()
        };

        return item;
    }
}