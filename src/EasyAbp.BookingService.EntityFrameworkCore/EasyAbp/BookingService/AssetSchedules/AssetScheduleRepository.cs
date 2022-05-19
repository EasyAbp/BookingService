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

    /// <inheritdoc/>
    public async Task<List<AssetSchedule>> GetAssetSchedulesAsync(Guid assetId, DateTime startingDateTime,
        DateTime endingDateTime,
        PeriodUsable? policy = default, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = includeDetails ? await WithDetailsAsync() : await GetDbSetAsync();

        return await queryable.Where(x => x.AssetId == assetId)
            .Where(x =>
                !(x.StartingDateTime >= endingDateTime // new schedule is on the left side of the old schedule in timeline
                  || x.EndingDateTime <= startingDateTime)) // new schedule is on the right side of the old schedule in timeline
            .WhereIf(policy.HasValue, x => x.PeriodUsable == policy.Value)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}