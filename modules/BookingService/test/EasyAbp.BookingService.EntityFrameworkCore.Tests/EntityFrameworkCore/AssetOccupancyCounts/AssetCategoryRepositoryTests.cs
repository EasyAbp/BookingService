using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancyCounts;
using EasyAbp.BookingService.Assets;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.AssetOccupancyCounts
{
    public class AssetOccupancyCountRepositoryTests : BookingServiceEntityFrameworkCoreTestBase
    {
        private readonly IAssetOccupancyCountRepository _assetOccupancyCountRepository;

        public AssetOccupancyCountRepositoryTests()
        {
            _assetOccupancyCountRepository = GetRequiredService<IAssetOccupancyCountRepository>();
        }

        [Fact]
        public async Task GetList_Baseline_Test()
        {
            // Arrange
            var assetId1 = GuidGenerator.Create();
            var assetId2 = GuidGenerator.Create();
            var date1 = new DateTime(2022, 6, 19);
            var date2 = new DateTime(2022, 6, 20);

            await WithUnitOfWorkAsync(async () =>
            {
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date1, assetId1)[0]);
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date1, assetId2)[0]);
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date2, assetId1)[0]);
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date2, assetId2)[0]);
            });

            // Act
            var result = await WithUnitOfWorkAsync(() => _assetOccupancyCountRepository.GetListAsync(date1, assetId1));

            // Assert
            result.Count.ShouldBe(1);
            result[0].Date.ShouldBe(date1);
            result[0].AssetId.ShouldBe(assetId1);
        }

        [Fact]
        public async Task Get_Baseline_Test()
        {
            // Arrange
            var assetId1 = GuidGenerator.Create();
            var assetId2 = GuidGenerator.Create();
            var date1 = new DateTime(2022, 6, 19);
            var date2 = new DateTime(2022, 6, 20);
            var entity = CreateEntities(date1, assetId1)[0];

            await WithUnitOfWorkAsync(async () =>
            {
                await _assetOccupancyCountRepository.InsertAsync(entity);
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date1, assetId2)[0]);
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date2, assetId1)[0]);
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date2, assetId2)[0]);
            });

            // Act
            var result = await WithUnitOfWorkAsync(() => _assetOccupancyCountRepository.GetAsync(
                new AssetOccupancyCountKey(entity.Date, entity.AssetId, entity.StartingTime, entity.Duration)));

            // Assert
            result.ShouldNotBeNull();
            result.Date.ShouldBe(date1);
            result.AssetId.ShouldBe(assetId1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Find_Baseline_Test(bool isFound)
        {
            // Arrange
            var assetId1 = GuidGenerator.Create();
            var assetId2 = GuidGenerator.Create();
            var date1 = new DateTime(2022, 6, 19);
            var date2 = new DateTime(2022, 6, 20);
            var entity = CreateEntities(date1, assetId1)[0];

            await WithUnitOfWorkAsync(async () =>
            {
                if (isFound)
                {
                    await _assetOccupancyCountRepository.InsertAsync(entity);
                }

                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date1, assetId2)[0]);
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date2, assetId1)[0]);
                await _assetOccupancyCountRepository.InsertAsync(CreateEntities(date2, assetId2)[0]);
            });

            // Act
            var result = await WithUnitOfWorkAsync(() => _assetOccupancyCountRepository.FindAsync(
                new AssetOccupancyCountKey(entity.Date, entity.AssetId, entity.StartingTime, entity.Duration)));

            // Assert
            if (isFound)
            {
                result.ShouldNotBeNull();
                result.Date.ShouldBe(date1);
                result.AssetId.ShouldBe(assetId1);
            }
            else
            {
                result.ShouldBeNull();
            }
        }

        private List<AssetOccupancyCount> CreateEntities(DateTime date, Guid? assetId = default, int count = 1)
        {
            var list = new List<AssetOccupancyCount>();
            for (var i = 0; i < count; i++)
            {
                list.Add(new AssetOccupancyCount(default,
                    assetId ?? GuidGenerator.Create(),
                    nameof(Asset) + i,
                    date,
                    TimeSpan.Zero,
                    TimeSpan.FromHours(1),
                    1));
            }

            return list;
        }
    }
}