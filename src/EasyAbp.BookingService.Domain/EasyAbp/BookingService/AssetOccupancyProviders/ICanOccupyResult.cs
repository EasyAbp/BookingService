namespace EasyAbp.BookingService.AssetOccupancyProviders;

public interface ICanOccupyResult
{
    bool CanOccupy { get; }

    string ErrorCode { get; }
}