using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.BookingService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyRepository : EfCoreRepository<IBookingServiceDbContext, AssetOccupancy, Guid>,
    IAssetOccupancyRepository
{
    public AssetOccupancyRepository(IDbContextProvider<IBookingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<List<AssetOccupancy>> GetListAsync(DateTime date, Guid assetId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.Date == date && x.AssetId == assetId)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}