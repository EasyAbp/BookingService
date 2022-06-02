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

    public Guid? OccupierUserId { get; set; }

    public List<OccupyAssetInfoModel> Models { get; set; }
    
    public List<OccupyAssetByCategoryInfoModel> ByCategoryModels { get; set; }

    protected BulkOccupyAssetEto()
    {
        Models = new List<OccupyAssetInfoModel>();
        ByCategoryModels = new List<OccupyAssetByCategoryInfoModel>();
    }

    public BulkOccupyAssetEto(Guid? tenantId, Guid? occupierUserId, List<OccupyAssetInfoModel> models,
        List<OccupyAssetByCategoryInfoModel> byCategoryModels)
    {
        TenantId = tenantId;
        OccupierUserId = occupierUserId;
        Models = models ?? new List<OccupyAssetInfoModel>();
        ByCategoryModels = byCategoryModels ?? new List<OccupyAssetByCategoryInfoModel>();
    }
}