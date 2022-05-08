using System;
using EasyAbp.BookingService.Assets;
using JetBrains.Annotations;
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
    /// This value object describes the time range for assets that can occupy.
    /// Will fall back to <see cref="Asset"/> if the value here is <c>null</c>.
    /// </summary>
    [CanBeNull]
    public virtual TimeInAdvance TimeInAdvance { get; protected set; }

    protected AssetSchedule()
    {
    }

    public AssetSchedule(Guid id, Guid? tenantId, Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, [CanBeNull] TimeInAdvance timeInAdvance) : base(id)
    {
        TenantId = tenantId;
        AssetId = assetId;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        SchedulePolicy = schedulePolicy;
        TimeInAdvance = timeInAdvance;
    }

    public bool ContainsTimeRange(DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        var startTime = date + startingTime;
        var scheduleStartTime = Date + StartingTime;

        return (scheduleStartTime <= startTime) && ((scheduleStartTime + Duration) >= (startTime + duration));
    }

    public void Update(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance)
    {
        AssetId = assetId;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        SchedulePolicy = schedulePolicy;
        TimeInAdvance = timeInAdvance;
    }

    public DateTime GetStartingDateTime() => Date + StartingTime;
}