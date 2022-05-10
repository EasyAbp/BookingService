using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyResultEto : ExtensibleObject, IHasPeriodInfo, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid AssetId { get; set; }

    public string AssetName { get; set; }

    public string AssetDefinitionName { get; set; }

    public bool Success { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }

    public string OccupierName { get; set; }

    protected AssetOccupancyResultEto()
    {

    }

    public AssetOccupancyResultEto(Guid? tenantId, Guid assetId, string assetName, string assetDefinitionName,
        bool success, DateTime date, TimeSpan startingTime, TimeSpan duration, Guid? occupierUserId,
        string occupierName)
    {
        TenantId = tenantId;
        AssetId = assetId;
        AssetName = assetName;
        AssetDefinitionName = assetDefinitionName;
        Success = success;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        OccupierUserId = occupierUserId;
        OccupierName = occupierName;
    }
}