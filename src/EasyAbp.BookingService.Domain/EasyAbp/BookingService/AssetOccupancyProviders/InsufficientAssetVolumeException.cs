using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class InsufficientAssetVolumeException : BusinessException
{
    public InsufficientAssetVolumeException() : base(BookingServiceErrorCodes.InsufficientAssetVolume)
    {
    }
}