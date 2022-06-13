using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetOccupancyCounts;

public interface IAssetOccupancyCountRepository : IRepository<AssetOccupancyCount>
{
    Task<List<AssetOccupancyCount>> GetListAsync(DateTime date, Guid assetId,
        CancellationToken cancellationToken = default);

    Task<AssetOccupancyCount> GetAsync(AssetOccupancyCountKey key, CancellationToken cancellationToken = default);

    Task<AssetOccupancyCount> FindAsync(AssetOccupancyCountKey key, CancellationToken cancellationToken = default);
}