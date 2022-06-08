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

public class AssetManager : DomainService
{
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly BookingServiceOptions _options;

    public AssetManager(IAssetCategoryRepository assetCategoryRepository,
        IOptions<BookingServiceOptions> options)
    {
        _assetCategoryRepository = assetCategoryRepository;
        _options = options.Value;
    }

    public virtual async Task<Asset> CreateAsync(string name, [NotNull] string assetDefinitionName,
        AssetCategory assetCategory, Guid? periodSchemeId, PeriodUsable? defaultPeriodUsable, int volume, int priority,
        TimeInAdvance timeInAdvance, bool disabled)
    {
        Check.NotNullOrWhiteSpace(assetDefinitionName, nameof(assetDefinitionName));

        if (_options.AssetDefinitionConfigurations.All(x => x.Name != assetDefinitionName))
        {
            throw new AssetDefinitionNotExistsException(assetDefinitionName);
        }

        // assetDefinitionName should be same with AssetCategory
        if (assetCategory.AssetDefinitionName != assetDefinitionName)
        {
            throw new AssetDefinitionNameNotMatchException(assetDefinitionName, assetCategory.AssetDefinitionName);
        }

        return new Asset(GuidGenerator.Create(),
            CurrentTenant.Id,
            name,
            assetDefinitionName,
            assetCategory.Id,
            periodSchemeId,
            defaultPeriodUsable,
            volume,
            priority,
            timeInAdvance,
            disabled);
    }

    public async Task UpdateAsync(Asset asset, string name, string assetDefinitionName, AssetCategory assetCategory,
        Guid? periodSchemeId, PeriodUsable? defaultPeriodUsable, int volume, int priority, TimeInAdvance timeInAdvance,
        bool disabled)
    {
        Check.NotNullOrWhiteSpace(assetDefinitionName, nameof(assetDefinitionName));

        if (_options.AssetDefinitionConfigurations.All(x => x.Name != assetDefinitionName))
        {
            throw new AssetDefinitionNotExistsException(assetDefinitionName);
        }

        // assetDefinitionName should be same with AssetCategory
        if (assetCategory.AssetDefinitionName != assetDefinitionName)
        {
            throw new AssetDefinitionNameNotMatchException(assetDefinitionName, assetCategory.AssetDefinitionName);
        }

        asset.Update(name,
            assetDefinitionName,
            assetCategory.Id,
            periodSchemeId,
            defaultPeriodUsable,
            volume,
            priority,
            timeInAdvance,
            disabled);
    }
}