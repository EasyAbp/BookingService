using EasyAbp.BookingService.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public class AssetPeriodSchemeRepository :
    EfCoreRepository<IBookingServiceDbContext, AssetPeriodScheme, AssetPeriodSchemeKey>, IAssetPeriodSchemeRepository
{
    public AssetPeriodSchemeRepository(IDbContextProvider<IBookingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }
}