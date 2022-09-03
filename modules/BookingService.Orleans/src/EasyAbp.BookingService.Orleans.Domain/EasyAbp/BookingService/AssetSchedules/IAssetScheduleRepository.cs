using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleRepository : IRepository<AssetSchedule, Guid>
{
    Task<List<AssetSchedule>> GetListAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        CancellationToken cancellationToken = default);

    Task<AssetSchedule> FindAsync(DateTime date, Guid assetId, Guid periodSchemeId, Guid periodId,
        CancellationToken cancellationToken = default);

    Task<IQueryable<AssetSchedule>> FilterByAssetCategoryIdAsync(IQueryable<AssetSchedule> query, Guid assetCategoryId);
}