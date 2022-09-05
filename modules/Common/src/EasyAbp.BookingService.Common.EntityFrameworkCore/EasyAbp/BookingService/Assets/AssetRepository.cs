using System;
using EasyAbp.BookingService.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.Assets;

public class AssetRepository : EfCoreRepository<IBookingServiceCommonDbContext, Asset, Guid>, IAssetRepository
{
    public AssetRepository(IDbContextProvider<IBookingServiceCommonDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}