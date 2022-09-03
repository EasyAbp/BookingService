using System;
using EasyAbp.BookingService.Assets;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetSchedule : FullAuditedAggregateRoot<Guid>, IMultiTenant, IAssetSchedule
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual DateTime Date { get; protected set; }

    public virtual Guid AssetId { get; protected set; }

    public virtual Guid PeriodSchemeId { get; protected set; }

    public virtual Guid PeriodId { get; protected set; }

    /// <inheritdoc/>>
    public virtual PeriodUsable PeriodUsable { get; protected set; }

    /// <inheritdoc/>>
    public virtual TimeInAdvance TimeInAdvance { get; protected set; }

    protected AssetSchedule()
    {
    }

    internal AssetSchedule(Guid id, Guid? tenantId, DateTime date, Guid assetId, Guid periodSchemeId, Guid periodId,
        PeriodUsable periodUsable, [CanBeNull] TimeInAdvance timeInAdvance) : base(id)
    {
        TenantId = tenantId;
        Date = date;
        AssetId = assetId;
        PeriodSchemeId = periodSchemeId;
        PeriodId = periodId;
        PeriodUsable = periodUsable;
        TimeInAdvance = timeInAdvance;
    }

    public void Update(PeriodUsable periodUsable, TimeInAdvance timeInAdvance)
    {
        PeriodUsable = periodUsable;
        TimeInAdvance = timeInAdvance;
    }
}