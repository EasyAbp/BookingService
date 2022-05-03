using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancies
{
    public class AssetOccupancyAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetOccupancyAppService _assetOccupancyAppService;

        public AssetOccupancyAppServiceTests()
        {
            _assetOccupancyAppService = GetRequiredService<IAssetOccupancyAppService>();
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
