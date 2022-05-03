using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.Assets;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.Assets
{
    public class AssetRepositoryTests : BookingServiceEntityFrameworkCoreTestBase
    {
        private readonly IAssetRepository _assetRepository;

        public AssetRepositoryTests()
        {
            _assetRepository = GetRequiredService<IAssetRepository>();
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
