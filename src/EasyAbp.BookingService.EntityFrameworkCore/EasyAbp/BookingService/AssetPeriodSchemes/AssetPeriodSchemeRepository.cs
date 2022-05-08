using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.BookingService.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public class AssetPeriodSchemeRepository :
    EfCoreRepository<IBookingServiceDbContext, AssetPeriodScheme>, IAssetPeriodSchemeRepository
{
    public AssetPeriodSchemeRepository(IDbContextProvider<IBookingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public Task<List<AssetPeriodScheme>> GetListByAssetIdAsync(Guid assetId, DateTime startingDate, DateTime endDate,
        bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return GetListAsync(
            x =>
                x.AssetId == assetId
                && x.Date >= startingDate
                && x.Date <= endDate,
            includeDetails,
            cancellationToken);
    }
}