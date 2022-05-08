using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Volo.Abp.Domain.Values;

namespace EasyAbp.BookingService;

/// <summary>
/// This value object describes the time range for assets that can occupy.
/// </summary>
[Serializable]
public class TimeInAdvance : ValueObject
{
    /// <summary>
    /// The maximum number of days people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3</c> means can occupy AFTER "March 7, 2022, 0:00".
    /// Value <c>0</c> means can occupy AFTER "March 10, 2022, 0:00".
    /// Value <c>-1</c> means can not occupy.
    /// Only the value with an EARLIER time of this property and the <see cref="MaxTimespanInAdvance"/> property will take effect.
    /// </summary>
    public int MaxDaysInAdvance { get; set; }

    /// <summary>
    /// The maximum timespan people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3-days</c> means can occupy AFTER "March 7, 2022, 10:00".
    /// Value <c>0</c> means can not occupy.
    /// Only the value with an EARLIER time of this property and the <see cref="MaxDaysInAdvance"/> property will take effect.
    /// </summary>
    public TimeSpan MaxTimespanInAdvance { get; set; }

    /// <summary>
    /// The minimum number of days people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3</c> means can occupy BEFORE "March 7, 2022, 0:00".
    /// Value <c>0</c> means can occupy BEFORE "March 10, 2022, 0:00".
    /// Value <c>-1</c> or <c>null</c> means unlimited.
    /// Only the value with a LATER time of this property and the <see cref="MinTimespanInAdvance"/> property will take effect.
    /// Null values have the lowest priority.
    /// </summary>
    public int? MinDaysInAdvance { get; set; }

    /// <summary>
    /// The minimum timespan people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3-days</c> means can occupy BEFORE "March 7, 2022, 10:00".
    /// Value <c>0</c> or <c>null</c> means unlimited.
    /// Only the value with a LATER time of this property and the <see cref="MinDaysInAdvance"/> property will take effect.
    /// Null values have the lowest priority.
    /// </summary>
    public TimeSpan? MinTimespanInAdvance { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return MaxDaysInAdvance;
        yield return MaxTimespanInAdvance;
        yield return MinDaysInAdvance;
        yield return MinTimespanInAdvance;
    }

    public bool CanBook(TimeSpan timeSpan)
    {
        if (MaxDaysInAdvance == -1 || MaxTimespanInAdvance == TimeSpan.Zero)
        {
            return false;
        }

        var max = GetMaxTimespanInAdvance();
        if (timeSpan > max)
        {
            return false;
        }

        var min = GetMinTimespanInAdvance();
        if (min.HasValue && timeSpan < min.Value)
        {
            return false;
        }

        return true;
    }

    [CanBeNull]
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