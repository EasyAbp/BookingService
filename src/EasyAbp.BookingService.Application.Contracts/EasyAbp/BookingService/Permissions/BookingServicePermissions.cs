using Volo.Abp.Reflection;

namespace EasyAbp.BookingService.Permissions;

public class BookingServicePermissions
{
    public const string GroupName = "EasyAbp.BookingService";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(BookingServicePermissions));
    }

        public class AssetCategory
        {
            public const string Default = GroupName + ".AssetCategory";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class AssetOccupancy
        {
            public const string Default = GroupName + ".AssetOccupancy";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class AssetPeriodScheme
        {
            public const string Default = GroupName + ".AssetPeriodScheme";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Asset
        {
            public const string Default = GroupName + ".Asset";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class AssetSchedule
        {
            public const string Default = GroupName + ".AssetSchedule";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class PeriodScheme
        {
            public const string Default = GroupName + ".PeriodScheme";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
}
