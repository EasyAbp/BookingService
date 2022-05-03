using EasyAbp.Abp.Trees;
using EasyAbp.BookingService.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategoryRepository : EfCoreTreeRepository<IBookingServiceDbContext, AssetCategory>,
    IAssetCategoryRepository
{
    public AssetCategoryRepository(IDbContextProvider<IBookingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }
}