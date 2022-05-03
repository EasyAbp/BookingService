using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.AssetOccupancies
{
    public class AssetOccupancyRepositoryTests : BookingServiceEntityFrameworkCoreTestBase
    {
        private readonly IAssetOccupancyRepository _assetOccupancyRepository;

        public AssetOccupancyRepositoryTests()
        {
            _assetOccupancyRepository = GetRequiredService<IAssetOccupancyRepository>();
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
