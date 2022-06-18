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

    Task<bool> CanOccupyAsync(OccupyAssetInfoModel model);

    Task<bool> CanOccupyByCategoryAsync(OccupyAssetByCategoryInfoModel model);

    Task<bool> CanBulkOccupyAsync(List<OccupyAssetInfoModel> models,
        List<OccupyAssetByCategoryInfoModel> byCategoryModels);

    Task<(ProviderAssetOccupancyModel, AssetOccupancy)> OccupyAsync(OccupyAssetInfoModel model, Guid? occupierUserId);

    Task<(ProviderAssetOccupancyModel, AssetOccupancy)> OccupyByCategoryAsync(OccupyAssetByCategoryInfoModel model,
        Guid? occupierUserId);

    Task<List<(ProviderAssetOccupancyModel, AssetOccupancy)>> BulkOccupyAsync(List<OccupyAssetInfoModel> models,
        List<OccupyAssetByCategoryInfoModel> byCategoryModels, Guid? occupierUserId);
}