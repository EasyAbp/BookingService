namespace EasyAbp.BookingService.AssetOccupancyProviders;

public interface IOccupyingBaseInfo : IOccupyingTimeInfo
{
    int Volume { get; }
}