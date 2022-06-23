using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.Assets;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancies
{
    public class AssetOccupancyAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetOccupancyAppService _assetOccupancyAppService;
        private readonly IAssetOccupancyRepository _assetOccupancyRepository;

        public AssetOccupancyAppServiceTests()
        {
            _assetOccupancyAppService = GetRequiredService<IAssetOccupancyAppService>();
            _assetOccupancyRepository = GetRequiredService<IAssetOccupancyRepository>();
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
                _assetOccupancyRepository.InsertManyAsync(entities));

            foreach (var input in GetInputs(dates, assetIds))
            {
                // Act
                var result = await WithUnitOfWorkAsync(() => _assetOccupancyAppService.GetListAsync(input));

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

        private static IEnumerable<GetAssetOccupanciesRequestDto> GetInputs(IEnumerable<DateTime> dates,
            IEnumerable<Guid> assetIds)
        {
            foreach (var date in new DateTime?[] { null }.Concat(dates.Select(x => (DateTime?)x)))
            {
                foreach (var assetId in new Guid?[] { null }.Concat(assetIds.Select(x => (Guid?)x)))
                {
                    yield return new GetAssetOccupanciesRequestDto
                    {
                        Date = date,
                        AssetId = assetId,
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    };
                }
            }
        }

        private Task<List<AssetOccupancy>> CreateEntitiesAsync(HashSet<DateTime> dates,
            HashSet<Guid> assetIds)
        {
            var list = new List<AssetOccupancy>();
            var i = 0;
            foreach (var date in dates)
            {
                foreach (var assetId in assetIds)
                {
                    list.Add(new AssetOccupancy(GuidGenerator.Create(),
                        default,
                        assetId,
                        nameof(Asset) + i++,
                        AssetDefinition.Name,
                        1, date,
                        TimeSpan.Zero,
                        TimeSpan.FromHours(1),
                        default,
                        default));
                }
            }

            return Task.FromResult(list);
        }
    }
}