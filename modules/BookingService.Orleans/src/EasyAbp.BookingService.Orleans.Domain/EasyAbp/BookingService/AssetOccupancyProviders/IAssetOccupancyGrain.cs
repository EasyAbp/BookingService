using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public interface IAssetOccupancyGrain : IGrainWithGuidCompoundKey
{
    Task<List<ProviderAssetOccupancyModel>> GetAssetOccupanciesAsync();
    Task<ProviderAssetOccupancyModel> OccupyAsync(ProviderOccupyingInfoModel model);
    Task<bool> TryRollBackOccupancyAsync(ProviderAssetOccupancyModel model);
}