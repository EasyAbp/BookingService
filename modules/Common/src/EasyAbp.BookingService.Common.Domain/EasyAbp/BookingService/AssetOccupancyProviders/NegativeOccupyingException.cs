using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class NegativeOccupyingException : BusinessException
{
    public NegativeOccupyingException(int volume) : base(BookingServiceErrorCodes.NegativeOccupying)
    {
        WithData(nameof(volume), volume);
    }
}