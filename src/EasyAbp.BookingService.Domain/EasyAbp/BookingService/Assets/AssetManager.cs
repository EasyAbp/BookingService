using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetSchedules;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.Assets;

public class AssetManager : DomainService, IUnitOfWorkEnabled
{
    private readonly IAssetRepository _repository;
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly BookingServiceOptions _options;

    public AssetManager(IAssetRepository repository,
        IAssetCategoryRepository assetCategoryRepository,
        IOptions<BookingServiceOptions> options)
    {
        _repository = repository;
        _assetCategoryRepository = assetCategoryRepository;
        _options = options.Value;
    }

    public virtual async Task<Asset> CreateAsync(string name, [NotNull] string assetDefinitionName,
        Guid assetCategoryId,
        Guid? periodSchemeId,
        AssetSchedulePolicy? defaultSchedulePolicy,
        int priority, TimeInAdvance timeInAdvance, bool disabled)
    {
        Check.NotNullOrWhiteSpace(assetDefinitionName, nameof(assetDefinitionName));

        if (_options.AssetDefinitionConfigurations.All(x => x.Name != assetDefinitionName))
        {
            throw new AssetDefinitionNotExistsException(assetDefinitionName);
        }

        var category = await _assetCategoryRepository.GetAsync(assetCategoryId);

        // assetDefinitionName should be same with AssetCategory
        if (category.AssetDefinitionName != assetDefinitionName)
        {
            throw new AssetDefinitionNameNotMatchException(assetDefinitionName, category.AssetDefinitionName);
        }

        return new Asset(GuidGenerator.Create(),
            CurrentTenant.Id,
            name,
            assetDefinitionName,
            assetCategoryId,
            periodSchemeId,
            defaultSchedulePolicy,
            priority,
            timeInAdvance,
            disabled);
    }

    public async Task UpdateAsync(Asset asset, string name, string assetDefinitionName, Guid assetCategoryId,
        Guid? periodSchemeId,
        AssetSchedulePolicy? defaultSchedulePolicy, int priority, TimeInAdvance timeInAdvance, bool disabled)
    {
        Check.NotNullOrWhiteSpace(assetDefinitionName, nameof(assetDefinitionName));

        if (_options.AssetDefinitionConfigurations.All(x => x.Name != assetDefinitionName))
        {
            throw new AssetDefinitionNotExistsException(assetDefinitionName);
        }

        var category = await _assetCategoryRepository.GetAsync(assetCategoryId);

        // assetDefinitionName should be same with AssetCategory
        if (category.AssetDefinitionName != assetDefinitionName)
        {
            throw new AssetDefinitionNameNotMatchException(assetDefinitionName, category.AssetDefinitionName);
        }

        asset.Update(name,
            assetDefinitionName,
            assetCategoryId,
            periodSchemeId,
            defaultSchedulePolicy,
            priority,
            timeInAdvance,
            disabled);
    }
}