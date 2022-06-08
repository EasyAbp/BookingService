using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyResultEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid RequestId { get; set; }

    public bool Success { get; set; }

    public AssetOccupancyInfoModel Model { get; set; }

    protected AssetOccupancyResultEto()
    {
    }

    public AssetOccupancyResultEto(Guid? tenantId, Guid requestId, bool success, AssetOccupancyInfoModel model)
    {
        TenantId = tenantId;
        RequestId = requestId;
        Success = success;
        Model = model;
    }
}