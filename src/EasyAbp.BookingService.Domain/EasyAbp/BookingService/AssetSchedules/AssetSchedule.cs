using System;
using EasyAbp.BookingService.Assets;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetSchedule : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid AssetId { get; protected set; }

    /// <summary>
    /// The start of this time frame.
    /// The starting time is included, example: [11:30, 12:00)
    /// </summary>
    public virtual DateTime StartingDateTime { get; protected set; }

    /// <summary>
    /// The end of this time frame.
    /// The ending time is excluded, example: [11:30, 12:00)
    /// </summary>
    public virtual DateTime EndingDateTime { get; protected set; }

    /// <summary>
    /// Accept or reject occupying within this time frame.
    /// </summary>
    public virtual PeriodUsable PeriodUsable { get; protected set; }

    /// <summary>
    /// This value object describes the time range for assets that can occupy.
    /// Will fall back to <see cref="Asset"/> if the value here is <c>null</c>.
    /// </summary>
    [CanBeNull]
    public virtual TimeInAdvance TimeInAdvance { get; protected set; }

    protected AssetSchedule()
    {
    }

    public AssetSchedule(Guid id, Guid? tenantId, Guid assetId, DateTime startingDateTime, DateTime endingDateTime,
        PeriodUsable periodUsable, [CanBeNull] TimeInAdvance timeInAdvance) : base(id)
    {
        TenantId = tenantId;
        AssetId = assetId;
        StartingDateTime = startingDateTime;
        EndingDateTime = endingDateTime;
        PeriodUsable = periodUsable;
        TimeInAdvance = timeInAdvance;
    }

    public void Update(Guid assetId, DateTime startingDateTime, DateTime endingDateTime,
        PeriodUsable periodUsable, TimeInAdvance timeInAdvance)
    {
        AssetId = assetId;
        StartingDateTime = startingDateTime;
        EndingDateTime = endingDateTime;
        PeriodUsable = periodUsable;
        TimeInAdvance = timeInAdvance;
    }
}