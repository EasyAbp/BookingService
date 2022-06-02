using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.BookingService.AssetPeriodSchemes
{
    public class AssetPeriodSchemeAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetPeriodSchemeAppService _assetPeriodSchemeAppService;

        public AssetPeriodSchemeAppServiceTests()
        {
            _assetPeriodSchemeAppService = GetRequiredService<IAssetPeriodSchemeAppService>();
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
