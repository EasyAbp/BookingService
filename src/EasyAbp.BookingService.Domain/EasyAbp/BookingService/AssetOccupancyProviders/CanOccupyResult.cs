namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class CanOccupyResult : ICanOccupyResult
{
    public static CanOccupyResult Success { get; } = new(true, default);

    public CanOccupyResult(bool canOccupy, string errorCode)
    {
        CanOccupy = canOccupy;
        ErrorCode = errorCode;
    }

    public bool CanOccupy { get; }

    public string ErrorCode { get; }
}