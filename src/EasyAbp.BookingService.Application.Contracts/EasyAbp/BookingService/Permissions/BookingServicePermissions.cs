using Volo.Abp.Reflection;

namespace EasyAbp.BookingService.Permissions;

public class BookingServicePermissions
{
    public const string GroupName = "EasyAbp.BookingService";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(BookingServicePermissions));
    }
}
