using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class CreateUpdatePeriodSchemeDto : ExtensibleObject
{
    public string Name { get; set; }

    public List<CreatePeriodDto> Periods { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var validationResult in base.Validate(validationContext))
        {
            yield return validationResult;
        }

        if (!Periods.IsNullOrEmpty()) // Check for start time duplication
        {
            foreach (var grouping in Periods.GroupBy(x => x.StartingTime)
                         .Where(x => x.Count() > 1))
            {
                yield return new ValidationResult(
                    $"Periods cannot have same start time: {grouping.Key}",
                    new[]
                    {
                        nameof(Periods)
                    }
                );
            }
        }
    }
}