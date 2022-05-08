using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleRepository : IRepository<AssetSchedule, Guid>
{
    Task<List<AssetSchedule>> GetAssetScheduleListAfterDateAsync(Guid assetId, DateTime date,
        bool includeDetails = false, CancellationToken cancellationToken = default);
}