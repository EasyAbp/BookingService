using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Assets;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace EasyAbp.BookingService.AssetPeriodSchemes
{
    public class AssetPeriodSchemeAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetPeriodSchemeAppService _assetPeriodSchemeAppService;
        private readonly IAssetPeriodSchemeRepository _assetPeriodSchemeRepository;

        public AssetPeriodSchemeAppServiceTests()
        {
            _assetPeriodSchemeAppService = GetRequiredService<IAssetPeriodSchemeAppService>();
            _assetPeriodSchemeRepository = GetRequiredService<IAssetPeriodSchemeRepository>();
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

            var periodSchemeIds = new HashSet<Guid>
            {
                GuidGenerator.Create()
            };

            var entities = await CreateEntitiesAsync(dates, assetIds, periodSchemeIds);

            await WithUnitOfWorkAsync(() =>
                _assetPeriodSchemeRepository.InsertManyAsync(entities));

            foreach (var input in GetInputs(dates, assetIds, periodSchemeIds))
            {
                // Act
                var result = await WithUnitOfWorkAsync(() => _assetPeriodSchemeAppService.GetListAsync(input));

                // Assert
                // ReSharper disable PossibleInvalidOperationException
                var expected = entities.WhereIf(input.PeriodSchemeId.HasValue,
                        x => x.PeriodSchemeId == input.PeriodSchemeId.Value)
                    .WhereIf(input.AssetId.HasValue, x => x.AssetId == input.AssetId.Value)
                    .WhereIf(input.Date.HasValue, x => x.Date == input.Date.Value)
                    .ToList();
                // ReSharper restore PossibleInvalidOperationException

                result.Items.Count.ShouldBe(expected.Count);
                foreach (var dto in result.Items)
                {
                    var entity = expected.FirstOrDefault(x => x.Date == dto.Date
                                                              && x.AssetId == dto.AssetId
                                                              && x.PeriodSchemeId == dto.PeriodSchemeId);
                    entity.ShouldNotBeNull();
                }
            }
        }

        private static IEnumerable<GetAssetPeriodSchemesRequestDto> GetInputs(IEnumerable<DateTime> dates,
            IEnumerable<Guid> assetIds,
            IEnumerable<Guid> periodSchemeIds)
        {
            foreach (var date in new DateTime?[] { null }.Concat(dates.Select(x => (DateTime?)x)))
            {
                foreach (var assetId in new Guid?[] { null }.Concat(assetIds.Select(x => (Guid?)x)))
                {
                    foreach (var periodSchemeId in new Guid?[] { null }.Concat(periodSchemeIds.Select(x => (Guid?)x)))
                    {
                        yield return new GetAssetPeriodSchemesRequestDto
                        {
                            Date = date,
                            AssetId = assetId,
                            PeriodSchemeId = periodSchemeId,
                            MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                        };
                    }
                }
            }
        }

        private Task<List<AssetPeriodScheme>> CreateEntitiesAsync(HashSet<DateTime> dates,
            HashSet<Guid> assetIds,
            HashSet<Guid> periodSchemeIds)
        {
            var list = new List<AssetPeriodScheme>();
            foreach (var date in dates)
            {
                foreach (var assetId in assetIds)
                {
                    foreach (var periodSchemeId in periodSchemeIds)
                    {
                        list.Add(new AssetPeriodScheme(new AssetPeriodSchemeKey
                        {
                            Date = date,
                            AssetId = assetId
                        }, default, periodSchemeId));
                    }
                }
            }

            return Task.FromResult(list);
        }
    }
}