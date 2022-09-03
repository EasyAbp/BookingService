using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.AssetOccupancyCounts;

public class AssetOccupancyCount : AggregateRoot, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid AssetId { get; protected set; }

    /// <summary>
    /// By default, it is generated with "CategoryDisplayName-AssetName".
    /// </summary>
    [NotNull]
    public virtual string Asset { get; protected set; }

    public virtual DateTime Date { get; protected set; }

    public virtual TimeSpan StartingTime { get; protected set; }

    public virtual TimeSpan Duration { get; protected set; }

    public virtual int Volume { get; protected set; }

    protected AssetOccupancyCount()
    {
    }

    public AssetOccupancyCount(Guid? tenantId, Guid assetId, [NotNull] string asset, DateTime date,
        TimeSpan startingTime, TimeSpan duration, int volume)
    {
        TenantId = tenantId;
        AssetId = assetId;
        Asset = asset;
        Date = date;
        StartingTime = startingTime;
        Duration = duration;
        Volume = volume;
    }

    public void ChangeVolume(int changedVolume)
    {
        if (Volume + changedVolume < 0)
        {
            throw new UnexpectedNegativeVolumeException(AssetId, Date, StartingTime, Duration,
                Volume,
                changedVolume);
        }

        checked
        {
            Volume += changedVolume;
        }
    }

    public override object[] GetKeys()
    {
        return new object[] { Date, AssetId, StartingTime, Duration };
    }
}