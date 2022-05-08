namespace EasyAbp.BookingService;

public static class BookingServiceErrorCodes
{
    //Add your business exception error codes here...

    public const string AssetDefinitionNotExists = "EasyAbp.BookingService:AssetDefinitionNotExists";
    public const string AssetDefinitionNameNotMatch = "EasyAbp.BookingService:AssetDefinitionNameNotMatch";
    public const string CanNotDeleteDefaultPeriodScheme = "EasyAbp.BookingService:CanNotDeleteDefaultPeriodScheme";
    public const string AssetNotExists = "EasyAbp.BookingService:AssetNotExists";
    public const string AssetScheduleExists = "EasyAbp.BookingService:AssetScheduleExists";
}