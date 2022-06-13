using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class ProviderAssetOccupancyModel : IOccupyingBaseInfo
{
    public Guid AssetId { get; set; }

    public int Volume { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public ProviderAssetOccupancyModel(Guid assetId, int volume, DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        AssetId = assetId;
        Volume = volume;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
    }

    public override string ToString()
    {
        return $"AssetId={AssetId}, Volume={Volume}, Date={Date}, StartingTime={StartingTime}, Duration={Duration}";
    }
}