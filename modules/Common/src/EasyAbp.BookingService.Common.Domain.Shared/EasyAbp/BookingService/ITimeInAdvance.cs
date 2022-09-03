using System;

namespace EasyAbp.BookingService;

public interface ITimeInAdvance
{
    /// <summary>
    /// The maximum number of days people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3</c> means can occupy AFTER "March 7, 2022, 0:00".
    /// Value <c>0</c> means can occupy AFTER "March 10, 2022, 0:00".
    /// Value <c>-1</c> means can not occupy.
    /// Only the value with an EARLIER time of this property and the <see cref="MaxTimespanInAdvance"/> property will take effect.
    /// </summary>
    int MaxDaysInAdvance { get; set; }

    /// <summary>
    /// The maximum timespan people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3-days</c> means can occupy AFTER "March 7, 2022, 10:00".
    /// Value <c>0</c> means can not occupy.
    /// Only the value with an EARLIER time of this property and the <see cref="MaxDaysInAdvance"/> property will take effect.
    /// </summary>
    TimeSpan MaxTimespanInAdvance { get; set; }

    /// <summary>
    /// The minimum number of days people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3</c> means can occupy BEFORE "March 7, 2022, 0:00".
    /// Value <c>0</c> means can occupy BEFORE "March 10, 2022, 0:00".
    /// Value <c>-1</c> or <c>null</c> means unlimited.
    /// Only the value with a LATER time of this property and the <see cref="MinTimespanInAdvance"/> property will take effect.
    /// Null values have the lowest priority.
    /// </summary>
    int? MinDaysInAdvance { get; set; }

    /// <summary>
    /// The minimum timespan people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3-days</c> means can occupy BEFORE "March 7, 2022, 10:00".
    /// Value <c>0</c> or <c>null</c> means unlimited.
    /// Only the value with a LATER time of this property and the <see cref="MinDaysInAdvance"/> property will take effect.
    /// Null values have the lowest priority.
    /// </summary>
    TimeSpan? MinTimespanInAdvance { get; set; }
}