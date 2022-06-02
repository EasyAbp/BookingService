using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.AssetCategories
{
    public class AssetCategoryRepositoryTests : BookingServiceEntityFrameworkCoreTestBase
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
