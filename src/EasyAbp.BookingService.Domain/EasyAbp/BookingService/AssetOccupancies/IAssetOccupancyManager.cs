using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.BookingService.AssetOccupancies;

public interface IAssetOccupancyManager
{
    Task<AssetOccupancy> CreateAsync(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId);

    Task UpdateAsync(AssetOccupancy entity, Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId);

    Task<List<AssetBookableDate>> SearchAssetBookableDatesAsync(Guid assetId, DateTime bookingDateTime,
        DateTime startingDate, int days);

    Task<List<CategoryBookableDate>> SearchCategoryBookableDatesAsync(Guid categoryId, DateTime bookingDateTime,
        DateTime startingDate, int days);
}