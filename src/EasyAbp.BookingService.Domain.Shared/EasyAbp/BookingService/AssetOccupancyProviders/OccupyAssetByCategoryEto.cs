using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

/// <summary>
/// Extra properties will fully map to <see cref="AssetOccupancyResultEto"/> by default.
/// </summary>
public class OccupyAssetByCategoryEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid RequestId { get; set; }

    public Guid? OccupierUserId { get; set; }

    public OccupyAssetByCategoryInfoModel Model { get; set; }

    protected OccupyAssetByCategoryEto()
    {
    }

    public OccupyAssetByCategoryEto(Guid? tenantId, Guid requestId, Guid? occupierUserId,
        OccupyAssetByCategoryInfoModel model)
    {
        TenantId = tenantId;
        RequestId = requestId;
        OccupierUserId = occupierUserId;
        Model = model;
    }
}