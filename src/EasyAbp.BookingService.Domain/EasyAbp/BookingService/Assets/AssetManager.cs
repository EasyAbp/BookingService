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

namespace EasyAbp.BookingService.Assets;

public class AssetManager : DomainService
{
    private readonly BookingServiceOptions _options;

    public AssetManager(IOptions<BookingServiceOptions> options)
    {
        _options = options.Value;
    }

    public virtual Task<Asset> CreateAsync(string name, [NotNull] string assetDefinitionName,
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

        return Task.FromResult(new Asset(GuidGenerator.Create(),
            CurrentTenant.Id,
            name,
            assetDefinitionName,
            assetCategory.Id,
            periodSchemeId,
            defaultPeriodUsable,
            volume,
            priority,
            timeInAdvance,
            disabled));
    }

    public Task UpdateAsync(Asset asset, string name, [NotNull] string assetDefinitionName, AssetCategory assetCategory,
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
        return Task.CompletedTask;
    }
}