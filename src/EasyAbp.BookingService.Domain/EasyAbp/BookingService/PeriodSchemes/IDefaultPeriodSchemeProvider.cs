using System.Threading.Tasks;
using JetBrains.Annotations;

namespace EasyAbp.BookingService.PeriodSchemes;

public interface IDefaultPeriodSchemeProvider
{
    [ItemCanBeNull]
    Task<PeriodScheme> GetAsync();

    Task ClearCacheAsync();
}