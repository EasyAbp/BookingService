using System;
using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyCounts;

public class AssetOccupancyCountVolumeCannotLessThanZeroException : BusinessException
{
    public AssetOccupancyCountVolumeCannotLessThanZeroException(Guid assetId, DateTime date, TimeSpan startingTime,
        TimeSpan duration, int volume, int changedVolume)
        : base(BookingServiceErrorCodes.AssetOccupancyCountVolumeCannotLessThanZero)
    {
        WithData(nameof(assetId), assetId);
        WithData(nameof(date), date);
        WithData(nameof(startingTime), startingTime);
        WithData(nameof(duration), duration);
        WithData(nameof(volume), volume);
        WithData(nameof(changedVolume), changedVolume);
    }
}