using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetSchedules;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Services;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategoryManager : DomainService, IAssetCategoryManager
{
    private readonly IAssetCategoryRepository _repository;
    private readonly BookingServiceOptions _options;

    public AssetCategoryManager(IAssetCategoryRepository repository,
        IOptions<BookingServiceOptions> options)
    {
        _repository = repository;
        _options = options.Value;
    }

    public async Task<AssetCategory> CreateAsync(Guid? parentId, string displayName, string assetDefinitionName,
        Guid? periodSchemeId,
        AssetSchedulePolicy? defaultSchedulePolicy, TimeInAdvance timeInAdvance, bool disabled)
    {
        if (_options.AssetDefinitions.All(x => x.Name != assetDefinitionName))
        {
            // TODO: throw ex?
            throw new AssetDefinitionNotExistsException(assetDefinitionName);
        }

        // TODO: Do we need to check periodScheme exists if periodSchemeId is not null?

        return new AssetCategory(GuidGenerator.Create(),
            CurrentTenant.Id,
            assetDefinitionName,
            periodSchemeId,
            defaultSchedulePolicy,
            parentId,
            displayName,
            timeInAdvance,
            disabled);
    }

    public async Task UpdateAsync(AssetCategory entity, Guid? parentId, string displayName, Guid? periodSchemeId,
        AssetSchedulePolicy? defaultSchedulePolicy, TimeInAdvance timeInAdvance, bool disabled)
    {
        // TODO: Do we need to check periodScheme exists if periodSchemeId is not null?

        entity.Update(parentId, displayName, periodSchemeId, defaultSchedulePolicy, timeInAdvance, disabled);
    }
}