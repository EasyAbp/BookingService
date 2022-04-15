using EasyAbp.BookingService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.BookingService.Permissions;

public class BookingServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BookingServicePermissions.GroupName, L("Permission:BookingService"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BookingServiceResource>(name);
    }
}
