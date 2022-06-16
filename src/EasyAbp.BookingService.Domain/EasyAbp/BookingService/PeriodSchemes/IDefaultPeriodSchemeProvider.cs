using System.Threading.Tasks;

namespace EasyAbp.BookingService.PeriodSchemes;

public interface IDefaultPeriodSchemeProvider
{
    Task<PeriodScheme> GetAsync();

    Task ClearCacheAsync();
}