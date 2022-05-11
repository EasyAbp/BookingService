using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

/// <summary>
/// Extra properties will fully map to <see cref="BulkAssetOccupancyResultEto"/> by default.
/// </summary>
public class BulkOccupyAssetByCategoryEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public List<OccupyAssetByCategoryInfoModel> Models { get; set; }

    protected BulkOccupyAssetByCategoryEto()
    {
    }

    public BulkOccupyAssetByCategoryEto(Guid? tenantId, List<OccupyAssetByCategoryInfoModel> models)
    {
        TenantId = tenantId;
        Models = models;
    }
}