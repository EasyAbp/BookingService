using EasyAbp.BookingService.AssetCategories;

namespace EasyAbp.BookingService.EntityFrameworkCore.AssetCategories
{
    public class AssetCategoryRepositoryTests : BookingServiceCommonEntityFrameworkCoreTestBase
    {
        private readonly IAssetCategoryRepository _assetCategoryRepository;

        public AssetCategoryRepositoryTests()
        {
            _assetCategoryRepository = GetRequiredService<IAssetCategoryRepository>();
        }

        /*
        [Fact]
        public async Task Test1()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                // Arrange

                // Act

                //Assert
            });
        }
        */
    }
}
