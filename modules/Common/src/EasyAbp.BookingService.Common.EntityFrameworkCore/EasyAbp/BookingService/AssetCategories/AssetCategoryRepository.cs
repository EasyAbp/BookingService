using EasyAbp.Abp.Trees;
using EasyAbp.BookingService.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategoryRepository : EfCoreTreeRepository<IBookingServiceCommonDbContext, AssetCategory>,
    IAssetCategoryRepository
{
    public AssetCategoryRepository(IDbContextProvider<IBookingServiceCommonDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }
}