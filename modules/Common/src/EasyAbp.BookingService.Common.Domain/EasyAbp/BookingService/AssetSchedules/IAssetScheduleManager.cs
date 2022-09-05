using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleManager
{
    Task<List<IAssetSchedule>> GetListAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        CancellationToken token = default);

    Task<bool> AnyByPeriodSchemeIdAsync(Guid periodSchemeId, CancellationToken token = default);
    
    Task<bool> AnyByPeriodIdAsync(Guid periodId, CancellationToken token = default);
}