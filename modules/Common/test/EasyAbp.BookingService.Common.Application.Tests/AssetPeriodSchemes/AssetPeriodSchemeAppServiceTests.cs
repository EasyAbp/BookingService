using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Assets;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace EasyAbp.BookingService.AssetPeriodSchemes
{
    public class AssetPeriodSchemeAppServiceTests : BookingServiceCommonApplicationTestBase
    {
        private readonly IAssetPeriodSchemeAppService _assetPeriodSchemeAppService;
        private readonly IAssetPeriodSchemeRepository _assetPeriodSchemeRepository;
        private readonly IObjectMapper<BookingServiceCommonApplicationModule> _objectMapper;

        public AssetPeriodSchemeAppServiceTests()
        {
            _assetPeriodSchemeAppService = GetRequiredService<IAssetPeriodSchemeAppService>();
            _assetPeriodSchemeRepository = GetRequiredService<IAssetPeriodSchemeRepository>();
            _objectMapper = GetRequiredService<IObjectMapper<BookingServiceCommonApplicationModule>>();
        }

        [Fact]
        public async Task Create_Should_Build_Entity_Through_MapToEntity()
        {
            // Arrange
            var assetId = GuidGenerator.Create();
            var periodSchemeId = GuidGenerator.Create();
            var date = new DateTime(2022, 6, 20);

            // Act
            var dto = await WithUnitOfWorkAsync(() => _assetPeriodSchemeAppService.CreateAsync(
                new CreateAssetPeriodSchemeDto
                {
                    AssetId = assetId,
                    PeriodSchemeId = periodSchemeId,
                    Date = date
                }));

            // Assert
            dto.AssetId.ShouldBe(assetId);
            dto.PeriodSchemeId.ShouldBe(periodSchemeId);
            dto.Date.ShouldBe(date);

            var entity = await WithUnitOfWorkAsync(() =>
                _assetPeriodSchemeRepository.GetAsync(x => x.AssetId == assetId && x.Date == date));
            entity.PeriodSchemeId.ShouldBe(periodSchemeId);
        }

        [Fact]
        public async Task Update_Should_Apply_Input_Through_MapToEntity()
        {
            // Arrange
            var assetId = GuidGenerator.Create();
            var periodSchemeId = GuidGenerator.Create();
            var date = new DateTime(2022, 6, 20);

            await WithUnitOfWorkAsync(() => _assetPeriodSchemeRepository.InsertAsync(
                new AssetPeriodScheme(new AssetPeriodSchemeKey { AssetId = assetId, Date = date }, default,
                    periodSchemeId)));

            // Act
            var dto = await WithUnitOfWorkAsync(() => _assetPeriodSchemeAppService.UpdateAsync(
                new AssetPeriodSchemeKey { AssetId = assetId, Date = date },
                new UpdateAssetPeriodSchemeDto { Date = date }));

            // Assert
            dto.AssetId.ShouldBe(assetId);
            dto.Date.ShouldBe(date);
            dto.PeriodSchemeId.ShouldBe(periodSchemeId);
        }

        [Fact]
        public void ObjectMapper_Should_Map_Entity_To_Dto_Including_ExtraProperties()
        {
            // Arrange
            var entity = new AssetPeriodScheme(
                new AssetPeriodSchemeKey { AssetId = GuidGenerator.Create(), Date = new DateTime(2022, 6, 20) },
                default,
                GuidGenerator.Create());
            entity.SetProperty("MyExtraProperty", "my-value");

            // Act
            var dto = _objectMapper.Map<AssetPeriodScheme, AssetPeriodSchemeDto>(entity);

            // Assert
            dto.AssetId.ShouldBe(entity.AssetId);
            dto.PeriodSchemeId.ShouldBe(entity.PeriodSchemeId);
            dto.Date.ShouldBe(entity.Date);
            dto.GetProperty<string>("MyExtraProperty").ShouldBe("my-value");
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