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

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var validationResult in base.Validate(validationContext))
        {
            yield return validationResult;
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