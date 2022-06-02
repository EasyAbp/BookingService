using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleRepository : IRepository<AssetSchedule, Guid>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="startingDateTime">Included</param>
    /// <param name="endingDateTime">Excluded</param>
    /// <param name="policy"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<AssetSchedule>> GetAssetScheduleListInScopeAsync(Guid assetId, DateTime startingDateTime, DateTime endingDateTime,
        PeriodUsable? policy = default, bool includeDetails = false,
        CancellationToken cancellationToken = default);
}