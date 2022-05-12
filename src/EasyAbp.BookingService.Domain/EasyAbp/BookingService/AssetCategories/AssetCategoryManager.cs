using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetSchedules;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategoryManager : DomainService, IUnitOfWorkEnabled
{
    private readonly IAssetCategoryRepository _repository;
    private readonly BookingServiceOptions _options;

    public AssetCategoryManager(IAssetCategoryRepository repository,
        IOptions<BookingServiceOptions> options)
    {
        _repository = repository;
        _options = options.Value;
    }

    public virtual Task<AssetCategory> CreateAsync(Guid? parentId, string displayName, string assetDefinitionName,
        Guid? periodSchemeId,
        AssetSchedulePolicy? defaultSchedulePolicy, TimeInAdvance timeInAdvance, bool disabled)
    {
        if (_options.AssetDefinitionConfigurations.All(x => x.Name != assetDefinitionName))
        {
            throw new AssetDefinitionNotExistsException(assetDefinitionName);
        }

        return Task.FromResult(new AssetCategory(GuidGenerator.Create(),
            CurrentTenant.Id,
            assetDefinitionName,
            periodSchemeId,
            defaultSchedulePolicy,
            parentId,
            displayName,
            timeInAdvance,
            disabled));
    }

    public virtual Task UpdateAsync(AssetCategory entity, Guid? parentId, string displayName, Guid? periodSchemeId,
        AssetSchedulePolicy? defaultSchedulePolicy, TimeInAdvance timeInAdvance, bool disabled)
    {
        entity.Update(parentId, displayName, periodSchemeId, defaultSchedulePolicy, timeInAdvance, disabled);
        return Task.CompletedTask;
    }
}