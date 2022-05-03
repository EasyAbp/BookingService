using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.BookingService.Assets
{
    public class AssetAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetAppService _assetAppService;

        public AssetAppServiceTests()
        {
            _assetAppService = GetRequiredService<IAssetAppService>();
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
