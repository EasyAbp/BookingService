using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleStore
{
    Task<List<AssetSchedule>> GetListAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        CancellationToken token = default);
}