using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.Assets;
using JetBrains.Annotations;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class BulkCanOccupyResult : ICanOccupyResult
{
    public static BulkCanOccupyResult Success { get; } = new(true, default);

    public BulkCanOccupyResult(bool canOccupy, string errorCode,
        [CanBeNull] Asset asset = default,
        [CanBeNull] AssetCategory category = default,
        [CanBeNull] IOccupyingBaseInfo occupyingBaseInfo = default)
    {
        CanOccupy = canOccupy;
        ErrorCode = errorCode;
        Asset = asset;
        Category = category;
        OccupyingBaseInfo = occupyingBaseInfo;
    }

    public bool CanOccupy { get; }

    public string ErrorCode { get; }

    [CanBeNull]
    public Asset Asset { get; }

    [CanBeNull]
    public AssetCategory Category { get; }

    [CanBeNull]
    public IOccupyingBaseInfo OccupyingBaseInfo { get; }
}