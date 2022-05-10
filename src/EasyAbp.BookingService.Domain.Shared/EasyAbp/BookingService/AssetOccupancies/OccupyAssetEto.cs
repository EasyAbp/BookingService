using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies;

public class OccupyAssetEto : ExtensibleObject, IMultiTenant
{
    public Guid? TenantId { get; set; }
    
    public Guid AssetId { get; set; }
    
    public virtual DateTime Date { get; protected set; }
    
    public virtual TimeSpan StartingTime { get; protected set; }
    
    public virtual TimeSpan Duration { get; protected set; }
    
    public virtual Guid? OccupierUserId { get; protected set; }
}