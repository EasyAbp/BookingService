using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules;
using Shouldly;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.AssetSchedules
{
    public class AssetScheduleRepositoryTests : BookingServiceEntityFrameworkCoreTestBase
    {
        private readonly IAssetScheduleRepository _assetScheduleRepository;
        private readonly AssetScheduleManager _assetScheduleManager;

        public AssetScheduleRepositoryTests()
        {
            _assetScheduleRepository = GetRequiredService<IAssetScheduleRepository>();
            _assetScheduleManager = GetRequiredService<AssetScheduleManager>();
        }

        [Fact]
        public async Task GetList_Baseline_Test()
        {
            // Arrange
            var assetId1 = GuidGenerator.Create();
            var assetId2 = GuidGenerator.Create();
            var periodSchemeId1 = GuidGenerator.Create();
            var periodSchemeId2 = GuidGenerator.Create();
            var date1 = new DateTime(2022, 6, 19);
            var date2 = new DateTime(2022, 6, 20);

            await WithUnitOfWorkAsync(async () =>
            {
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId1, periodSchemeId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId2, periodSchemeId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId1, periodSchemeId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId2, periodSchemeId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId1, periodSchemeId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId2, periodSchemeId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId1, periodSchemeId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId2, periodSchemeId2))[0]);
            });

            // Act
            var result = await WithUnitOfWorkAsync(() =>
                _assetScheduleRepository.GetListAsync(date1, assetId1, periodSchemeId1));

            // Assert
            result.Count.ShouldBe(1);
            result[0].Date.ShouldBe(date1);
            result[0].AssetId.ShouldBe(assetId1);
            result[0].PeriodSchemeId.ShouldBe(periodSchemeId1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Find_Baseline_Test(bool isFound)
        {
            // Arrange
            var assetId1 = GuidGenerator.Create();
            var assetId2 = GuidGenerator.Create();
            var periodSchemeId1 = GuidGenerator.Create();
            var periodSchemeId2 = GuidGenerator.Create();
            var periodId1 = GuidGenerator.Create();
            var periodId2 = GuidGenerator.Create();
            var date1 = new DateTime(2022, 6, 19);
            var date2 = new DateTime(2022, 6, 20);

            await WithUnitOfWorkAsync(async () =>
            {
                var entity = (await CreateEntitiesAsync(date1, assetId1, periodSchemeId1, periodId1))[0];
                if (isFound)
                {
                    await _assetScheduleRepository.InsertAsync(entity);
                }

                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId2, periodSchemeId1, periodId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId1, periodSchemeId1, periodId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId2, periodSchemeId1, periodId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId1, periodSchemeId2, periodId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId2, periodSchemeId2, periodId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId1, periodSchemeId2, periodId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId2, periodSchemeId2, periodId1))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId1, periodSchemeId1, periodId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId2, periodSchemeId1, periodId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId1, periodSchemeId1, periodId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId2, periodSchemeId1, periodId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId1, periodSchemeId2, periodId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date1, assetId2, periodSchemeId2, periodId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId1, periodSchemeId2, periodId2))[0]);
                await _assetScheduleRepository.InsertAsync(
                    (await CreateEntitiesAsync(date2, assetId2, periodSchemeId2, periodId2))[0]);
            });

            // Act
            var result = await WithUnitOfWorkAsync(() =>
                _assetScheduleRepository.FindAsync(date1, assetId1, periodSchemeId1, periodId1));

            // Assert
            if (isFound)
            {
                result.Date.ShouldBe(date1);
                result.AssetId.ShouldBe(assetId1);
                result.PeriodSchemeId.ShouldBe(periodSchemeId1);
                result.PeriodId.ShouldBe(periodId1);
            }
            else
            {
                result.ShouldBeNull();
            }
        }

        private async Task<List<AssetSchedule>> CreateEntitiesAsync(DateTime date, Guid? assetId = default,
            Guid? periodSchemeId = default,
            Guid? periodId = default,
            int count = 1)
        {
            var list = new List<AssetSchedule>();
            for (var i = 0; i < count; i++)
            {
                var entity = await _assetScheduleManager.CreateAsync(date,
                    assetId ?? GuidGenerator.Create(),
                    periodSchemeId ?? GuidGenerator.Create(),
                    periodId ?? GuidGenerator.Create(),
                    default,
                    default);
                list.Add(entity);
            }

            return list;
        }
    }
}