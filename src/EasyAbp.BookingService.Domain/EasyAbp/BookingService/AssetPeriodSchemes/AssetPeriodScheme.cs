using System;
using EasyAbp.BookingService.Assets;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public class AssetPeriodScheme : AuditedAggregateRoot<AssetPeriodSchemeKey>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    
    /// <summary>
    /// Will override the PeriodSchemeId of <see cref="Asset"/> entity.
    /// </summary>
    public virtual Guid PeriodSchemeId { get; protected set; }

    public AssetPeriodScheme([NotNull] AssetPeriodSchemeKey id, Guid? tenantId, Guid periodSchemeId) : base(id)
    {
        TenantId = tenantId;
        PeriodSchemeId = periodSchemeId;
    }
}