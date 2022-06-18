using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class DisabledAssetOrCategoryException : BusinessException
{
    public DisabledAssetOrCategoryException() : base(BookingServiceErrorCodes.DisabledAssetOrCategory)
    {
    }
}