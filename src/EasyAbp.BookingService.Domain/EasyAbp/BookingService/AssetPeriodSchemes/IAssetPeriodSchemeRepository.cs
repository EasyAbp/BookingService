using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public interface IAssetPeriodSchemeRepository : IRepository<AssetPeriodScheme>
{
    Task<List<AssetPeriodScheme>> GetListByAssetIdAsync(Guid assetId, DateTime startingDate, DateTime endDate,
        bool includeDetails = false, CancellationToken cancellationToken = default);
}