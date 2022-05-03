using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules;

namespace EasyAbp.BookingService.AssetCategories;

public interface IAssetCategoryManager
{
    Task<AssetCategory> CreateAsync(Guid? parentId, string displayName, string assetDefinitionName,
        Guid? periodSchemeId, AssetSchedulePolicy? defaultSchedulePolicy, TimeInAdvance timeInAdvance,
        bool disabled);

    Task UpdateAsync(AssetCategory entity, Guid? parentId, string displayName,
        Guid? periodSchemeId, AssetSchedulePolicy? defaultSchedulePolicy, TimeInAdvance timeInAdvance,
        bool disabled);
}