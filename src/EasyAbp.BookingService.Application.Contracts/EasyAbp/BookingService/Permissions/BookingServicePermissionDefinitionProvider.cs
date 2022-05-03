using EasyAbp.BookingService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.BookingService.Permissions;

public class BookingServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BookingServicePermissions.GroupName, L("Permission:BookingService"));

            var assetCategoryPermission = myGroup.AddPermission(BookingServicePermissions.AssetCategory.Default, L("Permission:AssetCategory"));
            assetCategoryPermission.AddChild(BookingServicePermissions.AssetCategory.Create, L("Permission:Create"));
            assetCategoryPermission.AddChild(BookingServicePermissions.AssetCategory.Update, L("Permission:Update"));
            assetCategoryPermission.AddChild(BookingServicePermissions.AssetCategory.Delete, L("Permission:Delete"));

            var assetOccupancyPermission = myGroup.AddPermission(BookingServicePermissions.AssetOccupancy.Default, L("Permission:AssetOccupancy"));
            assetOccupancyPermission.AddChild(BookingServicePermissions.AssetOccupancy.Create, L("Permission:Create"));
            assetOccupancyPermission.AddChild(BookingServicePermissions.AssetOccupancy.Update, L("Permission:Update"));
            assetOccupancyPermission.AddChild(BookingServicePermissions.AssetOccupancy.Delete, L("Permission:Delete"));

            var assetPeriodSchemePermission = myGroup.AddPermission(BookingServicePermissions.AssetPeriodScheme.Default, L("Permission:AssetPeriodScheme"));
            assetPeriodSchemePermission.AddChild(BookingServicePermissions.AssetPeriodScheme.Create, L("Permission:Create"));
            assetPeriodSchemePermission.AddChild(BookingServicePermissions.AssetPeriodScheme.Update, L("Permission:Update"));
            assetPeriodSchemePermission.AddChild(BookingServicePermissions.AssetPeriodScheme.Delete, L("Permission:Delete"));

            var assetPermission = myGroup.AddPermission(BookingServicePermissions.Asset.Default, L("Permission:Asset"));
            assetPermission.AddChild(BookingServicePermissions.Asset.Create, L("Permission:Create"));
            assetPermission.AddChild(BookingServicePermissions.Asset.Update, L("Permission:Update"));
            assetPermission.AddChild(BookingServicePermissions.Asset.Delete, L("Permission:Delete"));

            var assetSchedulePermission = myGroup.AddPermission(BookingServicePermissions.AssetSchedule.Default, L("Permission:AssetSchedule"));
            assetSchedulePermission.AddChild(BookingServicePermissions.AssetSchedule.Create, L("Permission:Create"));
            assetSchedulePermission.AddChild(BookingServicePermissions.AssetSchedule.Update, L("Permission:Update"));
            assetSchedulePermission.AddChild(BookingServicePermissions.AssetSchedule.Delete, L("Permission:Delete"));

            var periodSchemePermission = myGroup.AddPermission(BookingServicePermissions.PeriodScheme.Default, L("Permission:PeriodScheme"));
            periodSchemePermission.AddChild(BookingServicePermissions.PeriodScheme.Create, L("Permission:Create"));
            periodSchemePermission.AddChild(BookingServicePermissions.PeriodScheme.Update, L("Permission:Update"));
            periodSchemePermission.AddChild(BookingServicePermissions.PeriodScheme.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BookingServiceResource>(name);
    }
}
