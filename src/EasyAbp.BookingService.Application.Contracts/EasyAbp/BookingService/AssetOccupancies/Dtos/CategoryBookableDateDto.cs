using System;
using System.Collections.Generic;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

/// <summary>
/// Represent an category's bookable date.
/// </summary>
[Serializable]
public class CategoryBookableDateDto
{
    public DateTime Date { get; set; }

    /// <summary>
    /// Periods can be booked in this date.
    /// </summary>
    public List<CategoryBookablePeriodDto> BookablePeriods { get; set; } = new();
}