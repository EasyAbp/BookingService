using System;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetSchedules;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.Assets;

public class Asset : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    
    [NotNull]
    public virtual string Name { get; protected set; }
    
    [NotNull]
    public virtual string AssetDefinitionName { get; protected set; }
    
    public virtual Guid AssetCategoryId { get; protected set; }
    
    /// <summary>
    /// The property value from <see cref="AssetPeriodScheme"/> is preferred.
    /// Will fall back to <see cref="AssetCategory"/> if the value here is <c>null</c>.
    /// </summary>
    public virtual Guid? PeriodSchemeId { get; protected set; }
    
    /// <summary>
    /// This property determines whether assets can be occupied by default when there is no schedule created.
    /// Will fall back to <see cref="AssetCategory"/> if the value here is <c>null</c>.
    /// </summary>
    public virtual AssetSchedulePolicy? DefaultSchedulePolicy { get; protected set; }
    
    /// <summary>
    /// When occupying an Asset through AssetCategoryId, the Priority value determines to pick which Asset.
    /// </summary>
    public virtual int Priority { get; protected set; }
    
    /// <summary>
    /// This value object describes the time range for assets that can occupy.
    /// The property value from <see cref="AssetSchedule"/> is preferred.
    /// Will fall back to <see cref="AssetCategory"/> if the value here is <c>null</c>.
    /// </summary>
    [CanBeNull]
    public virtual TimeInAdvance TimeInAdvance { get; protected set; }
    
    public virtual bool Disabled { get; protected set; }

    protected Asset()
    {
        
    }

    public Asset(Guid id, Guid? tenantId, [NotNull] string name, [NotNull] string assetDefinitionName,
        Guid assetCategoryId, Guid? periodSchemeId, AssetSchedulePolicy? defaultSchedulePolicy, int priority,
        [CanBeNull] TimeInAdvance timeInAdvance, bool disabled) : base(id)
    {
        TenantId = tenantId;
        Name = name;
        AssetDefinitionName = assetDefinitionName;
        AssetCategoryId = assetCategoryId;
        PeriodSchemeId = periodSchemeId;
        DefaultSchedulePolicy = defaultSchedulePolicy;
        Priority = priority;
        TimeInAdvance = timeInAdvance;
        Disabled = disabled;
    }
}