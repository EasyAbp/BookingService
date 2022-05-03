using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.BookingService.Dtos;

[Serializable]
public class TimeInAdvanceDto : IValidatableObject
{
    /// <summary>
    /// The maximum number of days people can occupy assets in advance.
    /// Given we are occupying from "March 10, 2022, 10:00".
    /// Value <c>3</c> means can occupy AFTER "March 7, 2022, 0:00".
    /// Value <c>0</c> means can occupy AFTER "March 10, 2022, 0:00".
    /// Value <c>-1</c> means can not occupy.
    /// Only the value with an EARLIER time of this property and the <see cref="MaxTimespanInAdvance"/> property will take effect.
    /// </summary>
    [Required, Range(-1, int.MaxValue)]
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
    [Range(-1, int.MaxValue)]
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

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinDaysInAdvance > MaxDaysInAdvance)
        {
            yield return new ValidationResult(
                "MinDaysInAdvance should be less than MaxDaysInAdvance!",
                new[]
                {
                    nameof(MinDaysInAdvance), nameof(MaxDaysInAdvance)
                }
            );
        }

        if (MinTimespanInAdvance > MaxTimespanInAdvance)
        {
            yield return new ValidationResult(
                "MinTimespanInAdvance should be less than MaxTimespanInAdvance!",
                new[]
                {
                    nameof(MinTimespanInAdvance), nameof(MinTimespanInAdvance)
                }
            );
        }
    }
}