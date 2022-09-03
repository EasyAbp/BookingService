using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetOccupancies;

public static class AssetOccupancyEfCoreQueryableExtensions
{
    public static IQueryable<AssetOccupancy> IncludeDetails(this IQueryable<AssetOccupancy> queryable,
        bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}