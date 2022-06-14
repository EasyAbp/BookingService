using System;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class SearchBookingPeriodsInputDto
{
    /// <summary>
    /// The Id of asset to search
    /// </summary>
    [Required] public Guid AssetId { get; set; }
    
    /// <summary>
    /// The moment of a user is going to book
    /// </summary>
    [Required] public DateTime CurrentDateTime { get; set; }

    /// <summary>
    /// Search for the asset's bookable period on this date
    /// </summary>
    [Required] public DateTime TargetDate { get; set; }
}