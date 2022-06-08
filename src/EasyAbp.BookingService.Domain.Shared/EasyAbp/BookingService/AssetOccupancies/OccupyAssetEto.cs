using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

/// <summary>
/// Extra properties will fully map to <see cref="AssetOccupancyResultEto"/> by default.
/// </summary>
public class OccupyAssetEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid RequestId { get; set; }

    public Guid? OccupierUserId { get; set; }

    public OccupyAssetInfoModel Model { get; set; }

    protected OccupyAssetEto()
    {
    }

    public OccupyAssetEto(Guid? tenantId, Guid requestId, Guid? occupierUserId, OccupyAssetInfoModel model)
    {
        TenantId = tenantId;
        RequestId = requestId;
        OccupierUserId = occupierUserId;
        Model = model;
    }
}