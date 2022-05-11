using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyResultEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public bool Success { get; set; }

    public AssetOccupancyInfoModel Model { get; set; }

    protected AssetOccupancyResultEto()
    {
    }

    public AssetOccupancyResultEto(Guid? tenantId, bool success, AssetOccupancyInfoModel model)
    {
        TenantId = tenantId;
        Success = success;
        Model = model;
    }
}