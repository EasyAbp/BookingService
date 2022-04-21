using System;
using EasyAbp.BookingService.Assets;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetSchedule : FullAuditedAggregateRoot<Guid>, IHasPeriodInfo, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    
    public virtual Guid AssetId { get; protected set; }
    
    public virtual DateTime Date { get; protected set; }

    public virtual TimeSpan StartingTime { get; protected set; }
    
    public virtual TimeSpan Duration { get; protected set; }
    
    /// <summary>
    /// Accept or reject occupying within this time frame.
    /// </summary>
    public virtual AssetSchedulePolicy SchedulePolicy { get; protected set; }
    
    /// <summary>
    /// How many days in advance can you occupy the asset.
    /// Will fall back to <see cref="Asset"/> if the value here is <c>null</c>.
    /// </summary>
    public virtual int? DaysInAdvance { get; protected set; }

    protected AssetSchedule()
    {
        
    }

    public AssetSchedule(Guid id, Guid? tenantId, Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy) : base(id)
    {
        TenantId = tenantId;
        AssetId = assetId;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        SchedulePolicy = schedulePolicy;
    }
}