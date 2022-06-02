using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.BookingService.Dtos;

[Serializable]
public class TimeInAdvanceDto : IValidatableObject, ITimeInAdvance
{
    /// <inheritdoc/>
    [Required, Range(-1, int.MaxValue)]
    public int MaxDaysInAdvance { get; set; }

    /// <inheritdoc/>
    public TimeSpan MaxTimespanInAdvance { get; set; }

    /// <inheritdoc/>
    [Range(-1, int.MaxValue)]
    public int? MinDaysInAdvance { get; set; }

    /// <inheritdoc/>
    public TimeSpan? MinTimespanInAdvance { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinDaysInAdvance > MaxDaysInAdvance)
        {
            yield return new ValidationResult(
                "MinDaysInAdvance should be less than or equal to MaxDaysInAdvance!",
                new[]
                {
                    nameof(MinDaysInAdvance), nameof(MaxDaysInAdvance)
                }
            );
        }

        if (MinTimespanInAdvance > MaxTimespanInAdvance)
        {
            yield return new ValidationResult(
                "MinTimespanInAdvance should be less than or equal to MaxTimespanInAdvance!",
                new[]
                {
                    nameof(MinTimespanInAdvance), nameof(MinTimespanInAdvance)
                }
            );
        }
    }
}