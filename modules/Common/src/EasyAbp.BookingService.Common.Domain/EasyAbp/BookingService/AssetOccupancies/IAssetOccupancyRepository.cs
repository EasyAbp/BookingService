using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetOccupancies;

public interface IAssetOccupancyRepository : IRepository<AssetOccupancy, Guid>
{
    Task<List<AssetOccupancy>> GetListAsync(DateTime date, Guid assetId, CancellationToken cancellationToken = default);
}