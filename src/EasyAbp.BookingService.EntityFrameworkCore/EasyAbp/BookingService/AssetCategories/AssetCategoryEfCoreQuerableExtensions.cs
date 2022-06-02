using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetCategories;

public static class AssetCategoryEfCoreQueryableExtensions
{
    public static IQueryable<AssetCategory> IncludeDetails(this IQueryable<AssetCategory> queryable,
        bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}