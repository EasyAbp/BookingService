using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.BookingService.AssetSchedules
{
    public class AssetScheduleAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetScheduleAppService _assetScheduleAppService;

        public AssetScheduleAppServiceTests()
        {
            _assetScheduleAppService = GetRequiredService<IAssetScheduleAppService>();
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
