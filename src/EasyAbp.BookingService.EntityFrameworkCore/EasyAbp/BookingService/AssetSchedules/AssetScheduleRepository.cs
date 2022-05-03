using System;
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
}