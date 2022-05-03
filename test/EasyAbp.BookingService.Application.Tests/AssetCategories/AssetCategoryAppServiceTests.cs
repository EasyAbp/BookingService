using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.BookingService.AssetCategories
{
    public class AssetCategoryAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetCategoryAppService _assetCategoryAppService;

        public AssetCategoryAppServiceTests()
        {
            _assetCategoryAppService = GetRequiredService<IAssetCategoryAppService>();
        }

        /*
        [Fact]
        public async Task Test1()
        {
            // Arrange

            // Act

            // Assert
        }
        */
    }
}
