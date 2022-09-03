using System;
using EasyAbp.BookingService.Assets;
using JetBrains.Annotations;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetSchedule
{
    DateTime Date { get; }

    Guid AssetId { get; }

    Guid PeriodSchemeId { get; }

    Guid PeriodId { get; }

    /// <summary>
    /// Accept or reject occupying within this time frame.
    /// </summary>
    PeriodUsable PeriodUsable { get; }

    /// <summary>
    /// This value object describes the time range for assets that can occupy.
    /// Will fall back to <see cref="Asset"/> if the value here is <c>null</c>.
    /// </summary>
    [CanBeNull]
    TimeInAdvance TimeInAdvance { get; }
}