﻿using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetSchedules;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Services;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategoryManager : DomainService
{
    private readonly BookingServiceOptions _options;

    public AssetCategoryManager(IOptions<BookingServiceOptions> options)
    {
        _options = options.Value;
    }

    public virtual Task<AssetCategory> CreateAsync(Guid? parentId, string displayName, string assetDefinitionName,
        Guid? periodSchemeId,
        PeriodUsable? defaultPeriodUsable, TimeInAdvance timeInAdvance, bool disabled)
    {
        if (_options.AssetDefinitionConfigurations.All(x => x.Name != assetDefinitionName))
        {
            throw new AssetDefinitionNotExistsException(assetDefinitionName);
        }

        return Task.FromResult(new AssetCategory(GuidGenerator.Create(),
            CurrentTenant.Id,
            assetDefinitionName,
            periodSchemeId,
            defaultPeriodUsable,
            parentId,
            displayName,
            timeInAdvance,
            disabled));
    }

    public virtual Task UpdateAsync(AssetCategory entity, Guid? parentId, string displayName, Guid? periodSchemeId,
        PeriodUsable? defaultPeriodUsable, TimeInAdvance timeInAdvance, bool disabled)
    {
        entity.Update(parentId, displayName, periodSchemeId, defaultPeriodUsable, timeInAdvance, disabled);
        return Task.CompletedTask;
    }
}