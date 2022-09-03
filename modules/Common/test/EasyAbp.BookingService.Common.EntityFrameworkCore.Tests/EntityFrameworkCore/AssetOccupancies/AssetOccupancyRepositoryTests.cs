using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.Assets;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.AssetOccupancies
{
    public class AssetOccupancyRepositoryTests : BookingServiceCommonEntityFrameworkCoreTestBase
    {
        private readonly IAssetOccupancyRepository _assetOccupancyRepository;

        public AssetOccupancyRepositoryTests()
        {
            _assetOccupancyRepository = GetRequiredService<IAssetOccupancyRepository>();
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
                await _assetOccupancyRepository.InsertAsync(CreateEntities(date1, assetId1)[0]);
                await _assetOccupancyRepository.InsertAsync(CreateEntities(date1, assetId2)[0]);
                await _assetOccupancyRepository.InsertAsync(CreateEntities(date2, assetId1)[0]);
                await _assetOccupancyRepository.InsertAsync(CreateEntities(date2, assetId2)[0]);
            });

            // Act
            var result = await WithUnitOfWorkAsync(() => _assetOccupancyRepository.GetListAsync(date1, assetId1));
            
            // Assert
            result.Count.ShouldBe(1);
            result[0].Date.ShouldBe(date1);
            result[0].AssetId.ShouldBe(assetId1);
        }

        private List<AssetOccupancy> CreateEntities(DateTime date, Guid? assetId = default, int count = 1)
        {
            var list = new List<AssetOccupancy>();
            for (var i = 0; i < count; i++)
            {
                list.Add(new AssetOccupancy(GuidGenerator.Create(),
                    default,
                    assetId ?? GuidGenerator.Create(),
                    nameof(Asset) + i,
                    nameof(AssetDefinition),
                    1,
                    date,
                    TimeSpan.Zero,
                    TimeSpan.FromHours(1),
                    default,
                    default));
            }

            return list;
        }
    }
}