using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.AssetSchedules
{
    public class AssetScheduleRepositoryTests : BookingServiceEntityFrameworkCoreTestBase
    {
        private readonly IAssetScheduleRepository _assetScheduleRepository;

        public AssetScheduleRepositoryTests()
        {
            _assetScheduleRepository = GetRequiredService<IAssetScheduleRepository>();
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
