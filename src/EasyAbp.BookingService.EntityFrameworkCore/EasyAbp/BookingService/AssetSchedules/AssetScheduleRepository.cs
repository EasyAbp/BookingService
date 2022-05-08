using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.BookingService.EntityFrameworkCore;
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

    public Task<List<AssetSchedule>> GetAssetScheduleListAfterDateAsync(Guid assetId, DateTime date, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return GetListAsync(x => x.AssetId == assetId && x.Date >= date, includeDetails, cancellationToken);
    }
}