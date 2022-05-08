using System;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class SearchAssetBookableDatesRequestDto
{
    [Required] public Guid AssetId { get; set; }
    
    [Required] public DateTime BookingDateTime { get; set; }

    [Required] public DateTime StartingDate { get; set; }

    [Required, Range(1, 365)] public int Days { get; set; }
}