using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace EasyAbp.BookingService.AssetSchedules
{
    public class AssetScheduleAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetScheduleAppService _assetScheduleAppService;
        private readonly IAssetScheduleRepository _assetScheduleRepository;
        private readonly AssetScheduleManager _assetScheduleManager;

        public AssetScheduleAppServiceTests()
        {
            _assetScheduleAppService = GetRequiredService<IAssetScheduleAppService>();
            _assetScheduleRepository = GetRequiredService<IAssetScheduleRepository>();
            _assetScheduleManager = GetRequiredService<AssetScheduleManager>();
        }

        [Fact]
        public async Task GetList_Baseline_Test()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2022, 6, 20),
                new DateTime(2022, 6, 21)
            };
            var assetIds = new HashSet<Guid>
            {
                GuidGenerator.Create(),
                GuidGenerator.Create()
            };

            var entities = await CreateEntitiesAsync(dates, assetIds);

            await WithUnitOfWorkAsync(() =>
                _assetScheduleRepository.InsertManyAsync(entities));

            foreach (var input in GetInputs(dates, assetIds))
            {
                // Act
                var result = await WithUnitOfWorkAsync(() => _assetScheduleAppService.GetListAsync(input));

                // Assert
                // ReSharper disable PossibleInvalidOperationException
                var expected = entities.WhereIf(input.AssetId.HasValue,
                        x => x.AssetId == input.AssetId.Value)
                    .WhereIf(input.Date.HasValue,
                        x => x.Date == input.Date.Value)
                    .ToList();
                // ReSharper restore PossibleInvalidOperationException

                result.Items.Count.ShouldBe(expected.Count);
                foreach (var dto in result.Items)
                {
                    var entity = expected.FirstOrDefault(x => x.Id == dto.Id);
                    entity.ShouldNotBeNull();
                }
            }
        }

        private static IEnumerable<GetAssetSchedulesRequestDto> GetInputs(IEnumerable<DateTime> dates,
            IReadOnlyCollection<Guid> assetIds)
        {
            foreach (var date in new DateTime?[] { null }.Concat(dates.Select(x => (DateTime?)x)))
            {
                foreach (var assetId in new Guid?[] { null }.Concat(assetIds.Select(x => (Guid?)x)))
                {
                    yield return new GetAssetSchedulesRequestDto
                    {
                        Date = date,
                        AssetId = assetId,
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    };
                }
            }
        }

        private async Task<List<AssetSchedule>> CreateEntitiesAsync(HashSet<DateTime> dates,
            HashSet<Guid> assetIds)
        {
            var list = new List<AssetSchedule>();
            foreach (var date in dates)
            {
                foreach (var assetId in assetIds)
                {
                    list.Add(await _assetScheduleManager.CreateAsync(date, assetId,
                        GuidGenerator.Create(),
                        GuidGenerator.Create(),
                        default,
                        default));
                }
            }

            return list;
        }
    }
}