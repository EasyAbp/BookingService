using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class CreateAssetOccupancyByCategoryIdDto : ExtensibleObject
{
    public Guid AssetCategoryId { get; set; }
    
    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartingTime >= TimeSpan.FromDays(1))
        {
            yield return new ValidationResult(
                $"StartingTime should less than 24:00:00.",
                new[]
                {
                    nameof(StartingTime)
                }
            );
        }
    }
}