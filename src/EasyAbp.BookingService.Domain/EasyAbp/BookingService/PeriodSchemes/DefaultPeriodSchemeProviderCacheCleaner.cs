using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace EasyAbp.BookingService.PeriodSchemes;

public class DefaultPeriodSchemeProviderCacheCleaner : ILocalEventHandler<EntityChangedEventData<PeriodScheme>>,
    ITransientDependency
{
    private readonly IDefaultPeriodSchemeProvider _defaultPeriodSchemeProvider;

    public DefaultPeriodSchemeProviderCacheCleaner(IDefaultPeriodSchemeProvider defaultPeriodSchemeProvider)
    {
        _defaultPeriodSchemeProvider = defaultPeriodSchemeProvider;
    }
    
    public virtual async Task HandleEventAsync(EntityChangedEventData<PeriodScheme> eventData)
    {
        await _defaultPeriodSchemeProvider.ClearCacheAsync();
    }
}