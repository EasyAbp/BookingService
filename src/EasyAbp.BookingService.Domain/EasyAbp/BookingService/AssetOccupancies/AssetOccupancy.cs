using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancy : CreationAuditedAggregateRoot<Guid>, IHasPeriodInfo, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid AssetId { get; protected set; }

    /// <summary>
    /// By default, it is generated with "CategoryDisplayName-AssetName".
    /// </summary>
    [NotNull]
    public virtual string Asset { get; protected set; }

    [NotNull] public virtual string AssetDefinitionName { get; protected set; }

    public virtual int Volume { get; protected set; }

    public virtual DateTime Date { get; protected set; }

    public virtual TimeSpan StartingTime { get; protected set; }

    public virtual TimeSpan Duration { get; protected set; }

    public virtual Guid? OccupierUserId { get; protected set; }

    /// <summary>
    /// By default, it is the UserName of the occupier user.
    /// </summary>
    [CanBeNull]
    public virtual string OccupierName { get; protected set; }

    protected AssetOccupancy()
    {
    }

    public AssetOccupancy(Guid id, Guid? tenantId, Guid assetId, [NotNull] string asset,
        [NotNull] string assetDefinitionName, int volume, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId, [CanBeNull] string occupierName) : base(id)
    {
        TenantId = tenantId;
        AssetId = assetId;
        Asset = asset;
        AssetDefinitionName = assetDefinitionName;
        Volume = volume;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        OccupierUserId = occupierUserId;
        OccupierName = occupierName;
    }

    public DateTime GetStartingDateTime() => Date + StartingTime;

    public TimeSpan GetEndingTime() => StartingTime + Duration;
}