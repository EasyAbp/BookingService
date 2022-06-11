namespace EasyAbp.BookingService;

public static class BookingServiceErrorCodes
{
    //Add your business exception error codes here...

    public const string AssetDefinitionNotExists = "EasyAbp.BookingService:AssetDefinitionNotExists";
    public const string AssetDefinitionNameNotMatch = "EasyAbp.BookingService:AssetDefinitionNameNotMatch";
    public const string AssetNotExists = "EasyAbp.BookingService:AssetNotExists";
    public const string AssetScheduleExists = "EasyAbp.BookingService:AssetScheduleExists";
    public const string AssetDisabled = "EasyAbp.BookingService:AssetDisabled";
    public const string DefaultPeriodSchemeAlreadyExists = "EasyAbp.BookingService:DefaultPeriodSchemeAlreadyExists";
}