using System;
using EasyAbp.BookingService.EntityFrameworkCore;
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
}