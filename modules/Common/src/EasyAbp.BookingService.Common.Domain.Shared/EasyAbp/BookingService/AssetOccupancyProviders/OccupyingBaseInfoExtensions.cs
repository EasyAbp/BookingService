using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public static class OccupyingBaseInfoExtensions
{
    public static OccupyAssetInfoModel ToOccupyAssetInfoModel(this IOccupyingBaseInfo info, Guid assetId) =>
        new(assetId, info.Volume, info.Date, info.StartingTime, info.Duration);
}