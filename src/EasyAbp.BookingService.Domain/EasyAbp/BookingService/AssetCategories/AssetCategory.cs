using System;
using System.Collections.Generic;
using EasyAbp.Abp.Trees;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.PeriodSchemes;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetCategories;

public class AssetCategory : FullAuditedAggregateRoot<Guid>, ITree<AssetCategory>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Assets should have the same AssetDefinitionName as here.
    /// This should be readonly because Assets should have the same AssetDefinitionName as it's category. 
    /// </summary>
    [CanBeNull]
    public virtual string AssetDefinitionName { get; protected set; }

    /// <summary>
    /// This value only affects assets in this category, but not assets in children.
    /// The property value from <see cref="Asset"/> is preferred.
    /// Will fall back to <see cref="PeriodScheme"/> where the IsDefault property is <c>true</c> if the value here is <c>null</c>.
    /// </summary>
    public virtual Guid? PeriodSchemeId { get; protected set; }

    /// <summary>
    /// This property determines whether assets can be occupied by default when there is no schedule created.
    /// This value only affects assets in this category, but not assets in children.
    /// The property value from <see cref="Asset"/> is preferred.
    /// Will fall back to <see cref="AssetDefinition"/> if the value here is <c>null</c>.
    /// </summary>
    public virtual PeriodUsable? DefaultPeriodUsable { get; protected set; }

    /// <summary>
    /// This value object describes the time range for assets that can occupy.
    /// The property value from <see cref="Asset"/> is preferred.
    /// Will fall back to <see cref="AssetCategory"/> if the value here is <c>null</c>.
    /// </summary>
    [CanBeNull]
    public virtual TimeInAdvance TimeInAdvance { get; protected set; }

    public virtual bool Disabled { get; protected set; }

    #region Properties from ITree

    public virtual string Code { get; set; }

    public virtual int Level { get; set; }

    public virtual Guid? ParentId { get; set; }

    public virtual AssetCategory Parent { get; set; }

    public virtual ICollection<AssetCategory> Children { get; set; }

    public virtual string DisplayName { get; set; }

    #endregion

    protected AssetCategory()
    {
        Children = new List<AssetCategory>();
    }

    internal AssetCategory(Guid id, Guid? tenantId, [CanBeNull] string assetDefinitionName, Guid? periodSchemeId,
        PeriodUsable? defaultPeriodUsable, Guid? parentId, string displayName,
        [CanBeNull] TimeInAdvance timeInAdvance, bool disabled) : base(id)
    {
        TenantId = tenantId;
        AssetDefinitionName = assetDefinitionName;
        PeriodSchemeId = periodSchemeId;
        DefaultPeriodUsable = defaultPeriodUsable;
        ParentId = parentId;
        DisplayName = displayName;
        TimeInAdvance = timeInAdvance;
        Disabled = disabled;

        Children = new List<AssetCategory>();
    }

    internal void Update(Guid? parentId, string displayName, Guid? periodSchemeId,
        PeriodUsable? defaultPeriodUsable, TimeInAdvance timeInAdvance, bool disabled)
    {
        ParentId = parentId;
        DisplayName = displayName;
        PeriodSchemeId = periodSchemeId;
        DefaultPeriodUsable = defaultPeriodUsable;
        TimeInAdvance = timeInAdvance;
        Disabled = disabled;
    }
}