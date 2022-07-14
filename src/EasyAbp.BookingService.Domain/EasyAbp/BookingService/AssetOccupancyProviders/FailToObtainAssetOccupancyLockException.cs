using EasyAbp.BookingService.AssetCategories;
using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class FailToObtainAssetOccupancyLockException : BusinessException
{
    public FailToObtainAssetOccupancyLockException(AssetCategory category,
        IOccupyingTimeInfo model, int timeoutSeconds)
        : base(BookingServiceErrorCodes.FailToObtainAssetOccupancyLock)
    {
        WithData("categoryId", category.Id);
        WithData("date", model.Date);
        WithData("timeout", timeoutSeconds);
    }
}