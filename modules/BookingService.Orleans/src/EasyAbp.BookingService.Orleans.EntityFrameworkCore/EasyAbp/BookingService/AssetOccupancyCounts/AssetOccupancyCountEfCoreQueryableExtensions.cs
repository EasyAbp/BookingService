using System.Linq;

namespace EasyAbp.BookingService.AssetOccupancyCounts;

public static class AssetOccupancyCountEfCoreQueryableExtensions
{
    public static IQueryable<AssetOccupancyCount> IncludeDetails(this IQueryable<AssetOccupancyCount> queryable,
        bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}