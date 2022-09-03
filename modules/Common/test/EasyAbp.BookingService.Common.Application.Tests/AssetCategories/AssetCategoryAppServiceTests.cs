using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories.Dtos;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace EasyAbp.BookingService.AssetCategories
{
    public class AssetCategoryAppServiceTests : BookingServiceCommonApplicationTestBase
    {
        private readonly IAssetCategoryAppService _assetCategoryAppService;
        private readonly AssetCategoryManager _assetCategoryManager;
        private readonly IAssetCategoryRepository _assetCategoryRepository;

        public AssetCategoryAppServiceTests()
        {
            _assetCategoryAppService = GetRequiredService<IAssetCategoryAppService>();
            _assetCategoryRepository = GetRequiredService<IAssetCategoryRepository>();
            _assetCategoryManager = GetRequiredService<AssetCategoryManager>();
        }

        [Fact]
        public async Task GetList_Baseline_Test()
        {
            // Arrange
            var disabledSet = new HashSet<bool> { true, false };
            var displayNames = new HashSet<string> { "Name1", "Name2" };
            var assetDefinitionNames = new HashSet<string> { AnotherAssetDefinition.Name, AssetDefinition.Name };
            var (entities, parentId) =
                await CreateEntitiesAsync(disabledSet, displayNames, assetDefinitionNames);

            await WithUnitOfWorkAsync(() =>
                _assetCategoryRepository.InsertManyAsync(entities.Where(x => !x.ParentId.HasValue)));
            await WithUnitOfWorkAsync(() =>
                _assetCategoryRepository.InsertManyAsync(entities.Where(x => x.ParentId.HasValue)));

            foreach (var input in GetInputs(new HashSet<Guid?> { null, parentId }, disabledSet, assetDefinitionNames,
                         displayNames))
            {
                // Act
                var result = await WithUnitOfWorkAsync(() => _assetCategoryAppService.GetListAsync(input));

                // Assert
                var expected = entities.Where(x => x.ParentId == input.ParentId)
                    .WhereIf(input.Disabled.HasValue,
                        // ReSharper disable once PossibleInvalidOperationException
                        x => x.Disabled == input.Disabled.Value)
                    .WhereIf(!input.DisplayName.IsNullOrWhiteSpace(),
                        x => x.DisplayName == input.DisplayName)
                    .WhereIf(!input.AssetDefinitionName.IsNullOrWhiteSpace(),
                        x => x.AssetDefinitionName == input.AssetDefinitionName)
                    .ToList();

                result.Items.Count.ShouldBe(expected.Count);
                foreach (var dto in result.Items)
                {
                    var entity = expected.FirstOrDefault(x => x.Id == dto.Id);
                    entity.ShouldNotBeNull();
                }
            }
        }

        private static IEnumerable<GetAssetCategoriesRequestDto> GetInputs(HashSet<Guid?> parentIds,
            IReadOnlyCollection<bool> disabledSet, IReadOnlyCollection<string> assetDefinitionNames,
            IReadOnlyCollection<string> displayNames)
        {
            foreach (var parentId in parentIds)
            {
                foreach (var d in new bool?[] { null }.Concat(disabledSet.Select(x => (bool?)x)))
                {
                    foreach (var assetDefinitionName in assetDefinitionNames.Concat(new string[] { null }))
                    {
                        foreach (var displayName in displayNames.Concat(new string[] { null }))
                        {
                            yield return new GetAssetCategoriesRequestDto
                            {
                                Disabled = d,
                                DisplayName = displayName,
                                ParentId = parentId,
                                AssetDefinitionName = assetDefinitionName,
                                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                            };
                        }
                    }
                }
            }
        }

        private async Task<(List<AssetCategory>, Guid)> CreateEntitiesAsync(HashSet<bool> disabled,
            HashSet<string> displayNames,
            HashSet<string> assetDefinitionNames)
        {
            var list = new List<AssetCategory>();
            for (var i = 0; i < 2; i++)
            {
                foreach (var b in disabled)
                {
                    foreach (var displayName in displayNames)
                    {
                        foreach (var assetDefinitionName in assetDefinitionNames)
                        {
                            list.Add(await _assetCategoryManager.CreateAsync(i == 1 ? list[0].Id : null,
                                displayName,
                                assetDefinitionName,
                                default,
                                default,
                                default,
                                b));
                        }
                    }
                }
            }


            return (list, list[0].Id);
        }
    }
}