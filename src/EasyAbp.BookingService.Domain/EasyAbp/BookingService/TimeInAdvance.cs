using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Domain.Values;

namespace EasyAbp.BookingService;

/// <summary>
/// This value object describes the time range for assets that can occupy.
/// </summary>
[Serializable]
public class TimeInAdvance : ValueObject, ITimeInAdvance
{
    /// <inheritdoc/>
    public int MaxDaysInAdvance { get; set; }

    /// <inheritdoc/>
    public TimeSpan MaxTimespanInAdvance { get; set; }

    /// <inheritdoc/>
    public int? MinDaysInAdvance { get; set; }

    /// <inheritdoc/>
    public TimeSpan? MinTimespanInAdvance { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return MaxDaysInAdvance;
        yield return MaxTimespanInAdvance;
        yield return MinDaysInAdvance;
        yield return MinTimespanInAdvance;
    }

    /// <summary>
    /// Check whether you can occupy startingDateTime(March 10, 2022, 10:00) in bookingDateTime(March 8, 2022, 10:00)?
    /// </summary>
    /// <param name="startingDateTime">The starting datetime of period to occupy</param>
    /// <param name="bookingDateTime">The moment you want to occupy</param>
    /// <returns>True: can occupy</returns>
    public bool CanOccupy(DateTime startingDateTime, DateTime bookingDateTime)
    {
        var ts = startingDateTime - bookingDateTime;

        var max = GetMaxTimespanInAdvance();
        if (ts > max)
        {
            return false;
        }

        var min = GetMinTimespanInAdvance();
        return !min.HasValue || ts >= min.Value;
    }

    private TimeSpan? GetMinTimespanInAdvance()
    {
        if (MinDaysInAdvance.HasValue && MinTimespanInAdvance.HasValue)
        {
            return MinTimespanInAdvance.Value < TimeSpan.FromDays(MinDaysInAdvance.Value)
                ? MinTimespanInAdvance.Value
                : TimeSpan.FromDays(MinDaysInAdvance.Value);
        }
        else if (!MinDaysInAdvance.HasValue && MinTimespanInAdvance.HasValue)
        {
            return MinTimespanInAdvance.Value;
        }
        else if (!MinTimespanInAdvance.HasValue && MinDaysInAdvance.HasValue)
        {
            return TimeSpan.FromDays(MinDaysInAdvance.Value);
        }
        else
        {
            return default;
        }
    }

    private TimeSpan GetMaxTimespanInAdvance()
    {
        return MaxTimespanInAdvance > TimeSpan.FromDays(MaxDaysInAdvance)
            ? MaxTimespanInAdvance
            : TimeSpan.FromDays(MaxDaysInAdvance);
    }
}