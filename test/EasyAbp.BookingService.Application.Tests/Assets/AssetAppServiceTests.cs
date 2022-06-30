using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.Assets.Dtos;
using EasyAbp.BookingService.AssetSchedules;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace EasyAbp.BookingService.Assets
{
    public class AssetAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IAssetAppService _assetAppService;
        private readonly IAssetRepository _assetRepository;
        private readonly AssetCategoryManager _assetCategoryManager;
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly AssetManager _assetManager;

        public AssetAppServiceTests()
        {
            _assetAppService = GetRequiredService<IAssetAppService>();
            _assetRepository = GetRequiredService<IAssetRepository>();
            _assetManager = GetRequiredService<AssetManager>();
            _assetCategoryManager = GetRequiredService<AssetCategoryManager>();
            _assetCategoryRepository = GetRequiredService<IAssetCategoryRepository>();
        }

        [Fact]
        public async Task GetList_Baseline_Test()
        {
            // Arrange
            const string displayName = nameof(GetList_Baseline_Test);
            var assetDefinitionName = AssetDefinition.Name;
            var periodSchemeId = GuidGenerator.Create();
            const PeriodUsable defaultPeriodUsable = PeriodUsable.Accept;
            var timeInAdvance = new TimeInAdvance
            {
                MaxDaysInAdvance = 5
            };

            var assetCategory1 = await _assetCategoryManager.CreateAsync(default,
                displayName,
                assetDefinitionName,
                periodSchemeId,
                defaultPeriodUsable,
                timeInAdvance,
                default);

            var assetCategory2 = await _assetCategoryManager.CreateAsync(default,
                displayName,
                AnotherAssetDefinition.Name,
                periodSchemeId,
                defaultPeriodUsable,
                timeInAdvance,
                default);

            var assetCategories = new HashSet<AssetCategory>
            {
                assetCategory1,
                assetCategory2
            };

            var disabledSet = new HashSet<bool>
            {
                true,
                false
            };

            var names = new HashSet<string>
            {
                "Name1",
                "Name2"
            };

            var assetDefinitions = new HashSet<string>
            {
                AssetDefinition.Name,
                AnotherAssetDefinition.Name
            };

            var entities = await CreateEntitiesAsync(assetCategories, disabledSet, names, assetDefinitions);

            await WithUnitOfWorkAsync(() =>
                _assetCategoryRepository.InsertManyAsync(new[] { assetCategory1, assetCategory2 }));
            await WithUnitOfWorkAsync(() => _assetRepository.InsertManyAsync(entities));


            foreach (var input in GetInputs(assetCategories.Select(x => x.Id),
                         disabledSet, names, assetDefinitions))
            {
                // Act
                var result = await WithUnitOfWorkAsync(() => _assetAppService.GetListAsync(input));

                // Assert
                // ReSharper disable PossibleInvalidOperationException
                var expected = entities.WhereIf(input.AssetCategoryId.HasValue,
                        x => x.AssetCategoryId == input.AssetCategoryId.Value)
                    .WhereIf(input.Disabled.HasValue,
                        x => x.Disabled == input.Disabled.Value)
                    .WhereIf(!input.Name.IsNullOrWhiteSpace(),
                        x => x.Name == input.Name)
                    .WhereIf(!input.AssetDefinitionName.IsNullOrWhiteSpace(),
                        x => x.AssetDefinitionName == input.AssetDefinitionName)
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

        private static IEnumerable<GetAssetsRequestDto> GetInputs(IEnumerable<Guid> assetCategoryIds,
            IReadOnlyCollection<bool> disabledSet,
            IReadOnlyCollection<string> names, IReadOnlyCollection<string> assetDefinitions)
        {
            foreach (var assetCategoryId in new Guid?[] { null }.Concat(assetCategoryIds.Select(x => (Guid?)x)))
            {
                foreach (var disabled in new bool?[] { null }.Concat(disabledSet.Select(x => (bool?)x)))
                {
                    foreach (var name in new string[] { null }.Concat(names))
                    {
                        foreach (var assetDefinition in new string[] { null }.Concat(assetDefinitions))
                        {
                            yield return new GetAssetsRequestDto
                            {
                                AssetCategoryId = assetCategoryId,
                                Disabled = disabled,
                                Name = name,
                                AssetDefinitionName = assetDefinition,
                                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                            };
                        }
                    }
                }
            }
        }

        private async Task<List<Asset>> CreateEntitiesAsync(HashSet<AssetCategory> assetCategories,
            HashSet<bool> disabledSet,
            HashSet<string> names, HashSet<string> assetDefinitions)
        {
            var list = new List<Asset>();
            foreach (var assetCategory in assetCategories)
            {
                foreach (var disabled in disabledSet)
                {
                    foreach (var name in names)
                    {
                        foreach (var assetDefinition in assetDefinitions)
                        {
                            if (assetCategory.AssetDefinitionName == assetDefinition)
                            {
                                list.Add(await _assetManager.CreateAsync(name,
                                    // ReSharper disable once AssignNullToNotNullAttribute
                                    assetDefinition,
                                    assetCategory,
                                    default,
                                    default,
                                    1,
                                    1,
                                    default,
                                    disabled
                                ));
                            }
                        }
                    }
                }
            }

            return list;
        }
    }
}