using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.Assets;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public interface IAssetOccupancyProvider
{
    Task<List<PeriodOccupancyModel>> GetPeriodsAsync(Asset asset, AssetCategory categoryOfAsset,
        DateTime targetDate,
        DateTime? currentDateTime = default);

    Task<List<PeriodOccupancyModel>> GetPeriodsAsync(AssetCategory category,
        DateTime targetDate,
        DateTime? currentDateTime = default);

    Task CanOccupyAsync(OccupyAssetInfoModel model);

    Task CanOccupyByCategoryAsync(OccupyAssetByCategoryInfoModel model);

    Task CanBulkOccupyAsync(IEnumerable<OccupyAssetInfoModel> models,
        IEnumerable<OccupyAssetByCategoryInfoModel> byCategoryModels);

    Task<(ProviderAssetOccupancyModel, AssetOccupancy)> OccupyAsync(OccupyAssetInfoModel model, Guid? occupierUserId);

    Task<(ProviderAssetOccupancyModel, AssetOccupancy)> OccupyByCategoryAsync(OccupyAssetByCategoryInfoModel model,
        Guid? occupierUserId);

    Task<List<(ProviderAssetOccupancyModel, AssetOccupancy)>> BulkOccupyAsync(List<OccupyAssetInfoModel> models,
        List<OccupyAssetByCategoryInfoModel> byCategoryModels, Guid? occupierUserId);
}