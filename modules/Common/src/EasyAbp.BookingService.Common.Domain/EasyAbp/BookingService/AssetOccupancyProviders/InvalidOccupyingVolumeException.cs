using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class InvalidOccupyingVolumeException : BusinessException
{
    public InvalidOccupyingVolumeException(int volume) : base(BookingServiceErrorCodes.InvalidOccupyingVolume)
    {
        WithData(nameof(volume), volume);
    }
}