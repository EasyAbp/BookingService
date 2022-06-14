using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class BulkAssetOccupancyResultEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid RequestId { get; set; }

    public bool Success { get; set; }

    public List<AssetOccupancyInfoModel> Models { get; set; }

    protected BulkAssetOccupancyResultEto()
    {
        Models = new List<AssetOccupancyInfoModel>();
    }

    public BulkAssetOccupancyResultEto(Guid? tenantId, Guid requestId, bool success,
        List<AssetOccupancyInfoModel> models)
    {
        TenantId = tenantId;
        RequestId = requestId;
        Success = success;
        Models = models ?? new List<AssetOccupancyInfoModel>();
    }
}