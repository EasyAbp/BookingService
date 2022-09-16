using System;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.Assets;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class ProviderOccupyingInfoModel : IOccupyingBaseInfo
{
    public ProviderOccupyingInfoModel(Asset asset, AssetCategory categoryOfAsset, TimeSpan startingTime,
        TimeSpan duration, DateTime date, int volume)
    {
        if (volume <= 0)
        {
            throw new InvalidOccupyingVolumeException(volume);
        }

        Asset = asset;
        CategoryOfAsset = categoryOfAsset;
        StartingTime = startingTime;
        Duration = duration;
        Date = date;
        Volume = volume;
    }

    public Asset Asset { get; }

    public AssetCategory CategoryOfAsset { get; }

    public TimeSpan StartingTime { get; }

    public TimeSpan Duration { get; }

    public DateTime Date { get; }

    public int Volume { get; }
}