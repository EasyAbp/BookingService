using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public interface IAssetOccupyTransactionLock
{
    Task<IAsyncDisposable> TryAcquireAsync(IEnumerable<(Guid CategoryId, IOccupyingTimeInfo Model)> resources,
        TimeSpan timeout = default);
}