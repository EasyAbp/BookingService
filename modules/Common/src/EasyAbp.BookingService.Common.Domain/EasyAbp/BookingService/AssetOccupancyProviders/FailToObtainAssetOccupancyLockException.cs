using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class FailToObtainAssetOccupancyLockException : BusinessException
{
    public FailToObtainAssetOccupancyLockException()
        : base(BookingServiceErrorCodes.FailToObtainAssetOccupancyLock)
    {
    }
}