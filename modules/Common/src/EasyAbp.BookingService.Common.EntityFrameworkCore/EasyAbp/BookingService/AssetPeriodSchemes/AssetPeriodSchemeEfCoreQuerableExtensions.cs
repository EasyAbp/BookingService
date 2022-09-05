using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public static class AssetPeriodSchemeEfCoreQueryableExtensions
{
    public static IQueryable<AssetPeriodScheme> IncludeDetails(this IQueryable<AssetPeriodScheme> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}