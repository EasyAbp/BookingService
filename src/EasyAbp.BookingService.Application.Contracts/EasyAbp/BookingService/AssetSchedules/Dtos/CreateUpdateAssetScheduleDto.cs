using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EasyAbp.BookingService.Dtos;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetSchedules.Dtos;

[Serializable]
public class CreateUpdateAssetScheduleDto : ExtensibleObject
{
    public Guid AssetId { get; set; }

    public DateTime StartingDateTime { get; set; }

    public DateTime EndingDateTime { get; set; }

    public PeriodUsable PeriodUsable { get; set; }

    public TimeInAdvanceDto TimeInAdvance { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var validationResult in base.Validate(validationContext))
        {
            yield return validationResult;
        }

        if (EndingDateTime <= StartingDateTime)
        {
            yield return new ValidationResult(
                $"The EndingDateTime must greater than StartingDateTime, StartingDateTime: {StartingDateTime}, EndingDateTime: {EndingDateTime}",
                new[]
                {
                    nameof(StartingDateTime),
                    nameof(EndingDateTime)
                }
            );
        }
    }
}