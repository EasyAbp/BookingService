namespace EasyAbp.BookingService;

public static class BookingServiceUrls
{
    public static string HostUrl { get; set; } = string.Empty;

    public static string RouteBase { get; set; } = $"{HostUrl}/api/booking-service";

    public static string GetAssetListedDataSourceUrl { get; set; } = $"{RouteBase}/asset";

    public static string GetAssetSingleDataSourceUrl { get; set; } = $"{RouteBase}/asset/{{id}}";

    public static string GetAssetCategoryListedDataSourceUrl { get; set; } = $"{RouteBase}/asset-category";

    public static string GetAssetCategorySingleDataSourceUrl { get; set; } = $"{RouteBase}/asset-category/{{id}}";

    public static string GetPeriodSchemeListedDataSourceUrl { get; set; } = $"{RouteBase}/period-scheme";

    public static string GetPeriodSchemeSingleDataSourceUrl { get; set; } = $"{RouteBase}/period-scheme/{{id}}";
}