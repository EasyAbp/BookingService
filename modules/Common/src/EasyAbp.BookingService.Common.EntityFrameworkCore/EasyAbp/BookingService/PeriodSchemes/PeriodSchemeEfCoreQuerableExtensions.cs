using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyAbp.BookingService.PeriodSchemes
{
    public static class PeriodSchemeEfCoreQueryableExtensions
    {
        public static IQueryable<PeriodScheme> IncludeDetails(this IQueryable<PeriodScheme> queryable,
            bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Periods);
        }
    }
}