namespace EasyAbp.BookingService.AssetSchedules;

public enum AssetSchedulePolicy
{
    /// <summary>
    /// Periods are available.
    /// </summary>
    Accept,
    
    /// <summary>
    /// Periods are NOT available.
    /// </summary>
    Reject
}