using System;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

[Serializable]
public class AssetOccupyTransactionCacheItem
{
    public AssetOccupyTransactionCacheItem()
    {
    }

    public AssetOccupyTransactionCacheItem(Guid transactionId, long timestamp)
    {
        TransactionId = transactionId;
        Timestamp = timestamp;
    }

    public Guid TransactionId { get; set; }

    public long Timestamp { get; set; }

    public static string CalculateKey(Guid assetCategoryId, IOccupyingTimeInfo occupyingTimeInfo)
    {
        return $"{occupyingTimeInfo.Date:yyyyMMdd}|{assetCategoryId:N}";
    }
}