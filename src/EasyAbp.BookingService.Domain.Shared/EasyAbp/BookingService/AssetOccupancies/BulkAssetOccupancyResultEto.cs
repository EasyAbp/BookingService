using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

public class BulkAssetOccupancyResultEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public bool Success { get; set; }

    public List<AssetOccupancyInfoModel> Models { get; set; }
}