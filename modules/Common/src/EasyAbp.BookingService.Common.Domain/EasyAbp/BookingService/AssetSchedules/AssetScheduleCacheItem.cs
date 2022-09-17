using System;
using System.Collections.Generic;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleCacheItem
{
    public AssetScheduleCacheItem()
    {
        Items = new List<AssetSchedule>();
    }

    public AssetScheduleCacheItem(List<AssetSchedule> items)
    {
        Items = items;
    }

    public List<AssetSchedule> Items { get; set; }

    public static string CalculateKey(DateTime date, Guid assetId, Guid periodSchemeId, Guid? tenantId)
    {
        return $"d:{date:yyyy-MM-dd},a:{assetId:N},p:{periodSchemeId:N}{tenantId:N}";
    }
}