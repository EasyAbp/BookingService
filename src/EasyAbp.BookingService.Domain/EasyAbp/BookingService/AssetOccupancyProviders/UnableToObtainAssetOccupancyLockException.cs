using System;
using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class FailToObtainAssetOccupancyLockException : BusinessException
{
    public FailToObtainAssetOccupancyLockException(Guid categoryId,
        IOccupyingTimeInfo model)
        : base(BookingServiceErrorCodes.FailToObtainAssetOccupancyLock)
    {
        WithData("categoryId", categoryId);
        WithData("date", model.Date);
    }
}