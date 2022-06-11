using System;
using EasyAbp.BookingService.Assets;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public class AssetPeriodScheme : AuditedAggregateRoot, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Will override the PeriodSchemeId of <see cref="Asset"/> entity.
    /// </summary>
    public virtual Guid PeriodSchemeId { get; protected set; }

    public virtual Guid AssetId { get; protected set; }

    public virtual DateTime Date { get; protected set; }

    protected AssetPeriodScheme()
    {
    }

    public AssetPeriodScheme([NotNull] AssetPeriodSchemeKey id, Guid? tenantId, Guid periodSchemeId)
    {
        TenantId = tenantId;
        PeriodSchemeId = periodSchemeId;
        AssetId = id.AssetId;
        Date = id.Date;
    }

    public override object[] GetKeys()
    {
        return new object[] { AssetId, Date };
    }
}