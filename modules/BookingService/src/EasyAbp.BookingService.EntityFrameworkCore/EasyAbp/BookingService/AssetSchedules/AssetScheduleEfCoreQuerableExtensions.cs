using System.Linq;

namespace EasyAbp.BookingService.AssetSchedules;

public static class AssetScheduleEfCoreQueryableExtensions
{
    public static IQueryable<AssetSchedule> IncludeDetails(this IQueryable<AssetSchedule> queryable,
        bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}