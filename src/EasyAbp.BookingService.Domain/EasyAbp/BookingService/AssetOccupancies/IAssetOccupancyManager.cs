using System;
using System.Threading.Tasks;

namespace EasyAbp.BookingService.AssetOccupancies;

public interface IAssetOccupancyManager
{
    Task<AssetOccupancy> CreateAsync(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId);

    Task UpdateAsync(AssetOccupancy entity, Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId);
}