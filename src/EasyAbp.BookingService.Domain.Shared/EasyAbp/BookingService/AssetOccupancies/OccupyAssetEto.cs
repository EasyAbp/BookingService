using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

public class OccupyAssetEto : ExtensibleObject, IHasPeriodInfo, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }

    protected OccupyAssetEto()
    {

    }

    public OccupyAssetEto(Guid? tenantId, Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId)
    {
        TenantId = tenantId;
        AssetId = assetId;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        OccupierUserId = occupierUserId;
    }
}