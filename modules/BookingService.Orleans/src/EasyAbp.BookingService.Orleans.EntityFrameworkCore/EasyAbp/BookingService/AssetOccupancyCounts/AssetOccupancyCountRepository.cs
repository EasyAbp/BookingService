using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.BookingService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetOccupancyCounts;

public class AssetOccupancyCountRepository : EfCoreRepository<IBookingServiceDbContext, AssetOccupancyCount>,
    IAssetOccupancyCountRepository
{
    public AssetOccupancyCountRepository(IDbContextProvider<IBookingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<List<AssetOccupancyCount>> GetListAsync(DateTime date, Guid assetId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.Date == date && x.AssetId == assetId)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<AssetOccupancyCount> GetAsync(AssetOccupancyCountKey key,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(key, GetCancellationToken(cancellationToken));

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(AssetOccupancyCount), key);
        }

        return entity;
    }

    public virtual async Task<AssetOccupancyCount> FindAsync(AssetOccupancyCountKey key,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(x =>
                x.Date == key.Date &&
                x.AssetId == key.AssetId &&
                x.StartingTime == key.StartingTime &&
                x.Duration == key.Duration,
            cancellationToken: GetCancellationToken(cancellationToken));
    }
}