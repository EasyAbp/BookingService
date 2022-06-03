namespace EasyAbp.BookingService;

public static class BookingServiceUrls
{
    public const string RouteBase = "/api/booking-service";

    public const string GetAssetListedDataSourceUrl = $"{RouteBase}/asset";

    public const string GetAssetSingleDataSourceUrl = $"{RouteBase}/asset/{{id}}";

    public const string GetAssetCategoryListedDataSourceUrl = $"{RouteBase}/asset-category";

    public const string GetAssetCategorySingleDataSourceUrl = $"{RouteBase}/asset-category/{{id}}";

    public const string GetPeriodSchemeListedDataSourceUrl = $"{RouteBase}/period-scheme";

    public const string GetPeriodSchemeSingleDataSourceUrl = $"{RouteBase}/period-scheme/{{id}}";
}