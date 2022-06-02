using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.BookingService.PeriodSchemes
{
    public class PeriodSchemeAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IPeriodSchemeAppService _periodSchemeAppService;

        public PeriodSchemeAppServiceTests()
        {
            _periodSchemeAppService = GetRequiredService<IPeriodSchemeAppService>();
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
