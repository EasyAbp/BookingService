using System;
using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyCounts;

public class UnexpectedNegativeVolumeException : BusinessException
{
    public UnexpectedNegativeVolumeException(Guid assetId, DateTime date, TimeSpan startingTime,
        TimeSpan duration, int volume, int changedVolume)
        : base(BookingServiceErrorCodes.UnexpectedNegativeVolume)
    {
        WithData(nameof(assetId), assetId);
        WithData(nameof(date), date);
        WithData(nameof(startingTime), startingTime);
        WithData(nameof(duration), duration);
        WithData(nameof(volume), volume);
        WithData(nameof(changedVolume), changedVolume);
    }
}