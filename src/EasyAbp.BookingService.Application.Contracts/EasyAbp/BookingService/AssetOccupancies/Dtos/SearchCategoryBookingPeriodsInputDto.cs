using System;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class SearchCategoryBookingPeriodsInputDto
{
    /// <summary>
    /// The Id of categoryId to search
    /// </summary>
    [Required]
    public Guid CategoryId { get; set; }
    
    /// <summary>
    /// Override the value of Category.PeriodSchemeId
    /// </summary>
    public Guid? PeriodSchemeId { get; set; }

    /// <summary>
    /// The moment of a user is going to book
    /// </summary>
    public DateTime? CurrentDateTime { get; set; }

    /// <summary>
    /// Search for the asset's bookable period on this date
    /// </summary>
    [Required]
    public DateTime TargetDate { get; set; }
}