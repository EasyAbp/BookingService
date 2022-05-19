using System;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class SearchCategoryBookablePeriodsRequestDto
{
    /// <summary>
    /// The Id of categoryId to search
    /// </summary>
    [Required]
    public Guid CategoryId { get; set; }

    /// <summary>
    /// The moment of a user is going to book
    /// </summary>
    [Required]
    public DateTime BookingDateTime { get; set; }

    /// <summary>
    /// Search for the asset's bookable period on this date
    /// </summary>
    [Required]
    public DateTime SearchDate { get; set; }
}