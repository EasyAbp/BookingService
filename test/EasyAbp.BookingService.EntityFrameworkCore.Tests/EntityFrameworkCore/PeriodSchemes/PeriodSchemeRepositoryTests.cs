using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.PeriodSchemes
{
    public class PeriodSchemeRepositoryTests : BookingServiceEntityFrameworkCoreTestBase
    {
        private readonly IPeriodSchemeRepository _periodSchemeRepository;

        public PeriodSchemeRepositoryTests()
        {
            _periodSchemeRepository = GetRequiredService<IPeriodSchemeRepository>();
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
