using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules;

namespace EasyAbp.BookingService.Assets;

public interface IAssetManager
{
    Task<Asset> CreateAsync(string name, string assetDefinitionName, Guid assetCategoryId, Guid? periodSchemeId,
        AssetSchedulePolicy? defaultSchedulePolicy, int priority, TimeInAdvance timeInAdvance, bool disabled);

    Task UpdateAsync(Asset asset, string name, string assetDefinitionName, Guid assetCategoryId, Guid? periodSchemeId,
        AssetSchedulePolicy? defaultSchedulePolicy, int priority, TimeInAdvance timeInAdvance, bool disabled);
}