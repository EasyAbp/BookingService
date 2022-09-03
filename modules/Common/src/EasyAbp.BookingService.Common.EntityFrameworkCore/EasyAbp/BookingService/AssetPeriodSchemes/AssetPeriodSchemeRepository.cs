using EasyAbp.BookingService.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public class AssetPeriodSchemeRepository :
    EfCoreRepository<IBookingServiceCommonDbContext, AssetPeriodScheme>, IAssetPeriodSchemeRepository
{
    public AssetPeriodSchemeRepository(IDbContextProvider<IBookingServiceCommonDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }
}