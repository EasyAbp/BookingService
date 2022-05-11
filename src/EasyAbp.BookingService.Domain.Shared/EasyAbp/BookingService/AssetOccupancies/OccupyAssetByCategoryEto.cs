using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

/// <summary>
/// Extra properties will fully map to <see cref="AssetOccupancyResultEto"/> by default.
/// </summary>
public class OccupyAssetByCategoryEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public OccupyAssetByCategoryInfoModel Model { get; set; }

    protected OccupyAssetByCategoryEto()
    {
    }

    public OccupyAssetByCategoryEto(Guid? tenantId, OccupyAssetByCategoryInfoModel model)
    {
        TenantId = tenantId;
        Model = model;
    }
}