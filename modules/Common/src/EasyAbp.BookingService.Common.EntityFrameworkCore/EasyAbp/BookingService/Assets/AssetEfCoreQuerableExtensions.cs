using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyAbp.BookingService.Assets;

public static class AssetEfCoreQueryableExtensions
{
    public static IQueryable<Asset> IncludeDetails(this IQueryable<Asset> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}