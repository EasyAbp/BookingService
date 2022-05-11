using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

/// <summary>
/// Extra properties will fully map to <see cref="BulkAssetOccupancyResultEto"/> by default.
/// </summary>
public class BulkOccupyAssetEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public List<OccupyAssetInfoModel> Models { get; set; }

    protected BulkOccupyAssetEto()
    {
    }

    public BulkOccupyAssetEto(Guid? tenantId, List<OccupyAssetInfoModel> models)
    {
        TenantId = tenantId;
        Models = models;
    }
}