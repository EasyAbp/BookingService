using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.BookingService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleRepository : EfCoreRepository<IBookingServiceDbContext, AssetSchedule, Guid>,
    IAssetScheduleRepository
{
    public AssetScheduleRepository(IDbContextProvider<IBookingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<List<AssetSchedule>> GetListAsync(DateTime date, Guid assetId, Guid periodSchemeId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.Date == date && x.AssetId == assetId && x.PeriodSchemeId == periodSchemeId)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<AssetSchedule> FindAsync(DateTime date, Guid assetId, Guid periodSchemeId, Guid periodId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(
            x => x.Date == date && x.AssetId == assetId && x.PeriodSchemeId == periodSchemeId && x.PeriodId == periodId,
            cancellationToken);
    }

    public async Task<IQueryable<AssetSchedule>> FilterByAssetCategoryIdAsync(IQueryable<AssetSchedule> query,
        Guid assetCategoryId)
    {
        var assets = (await GetDbContextAsync()).Assets;
        query = from schedule in query
            from asset in assets.Where(x => x.Id == schedule.AssetId)
            where asset.AssetCategoryId == assetCategoryId
            select schedule;

        return query;
    }
}