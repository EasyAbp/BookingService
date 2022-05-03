using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetPeriodSchemes;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.AssetPeriodSchemes
{
    public class AssetPeriodSchemeRepositoryTests : BookingServiceEntityFrameworkCoreTestBase
    {
        private readonly IAssetPeriodSchemeRepository _assetPeriodSchemeRepository;

        public AssetPeriodSchemeRepositoryTests()
        {
            _assetPeriodSchemeRepository = GetRequiredService<IAssetPeriodSchemeRepository>();
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
