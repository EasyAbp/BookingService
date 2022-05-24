using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class CreatePeriodDto : ExtensibleObject, IHasPeriodInfo
{
    [Range(typeof(TimeSpan), "00:00:00", "23:59:59.999")]
    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    /// <summary>
    /// If you set it to <c>true</c>, this period could only be fully occupied.
    /// For example, given the period is from 10:00-11:00.
    /// You can occupy 10:00-10:30 when set to <c>true</c> but cannot when set to <c>false</c>.
    /// </summary>
    public bool Divisible { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var validationResult in base.Validate(validationContext))
        {
            yield return validationResult;
        }

        if (Divisible && StartingTime + Duration >= TimeSpan.FromDays(1))
        {
            // Divisible period cannot cross days.
            yield return new ValidationResult(
                $"The divisible period cannot cross days",
                new[]
                {
                    nameof(StartingTime),
                    nameof(Duration)
                }
            );
        }

        if (Duration < TimeSpan.Zero)
        {
            yield return new ValidationResult(
                $"The Duration of Period must be greater or equal 0, current StartingTime: {StartingTime}",
                new[]
                {
                    nameof(Duration)
                }
            );
        }
    }
}