namespace EasyAbp.BookingService;

public static class BookingServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "EasyAbpBookingService";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "EasyAbpBookingService";
}
