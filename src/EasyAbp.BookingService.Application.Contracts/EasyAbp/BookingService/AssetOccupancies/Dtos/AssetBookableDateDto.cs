using System;
using System.Collections.Generic;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

/// <summary>
/// Represent an asset's bookable date.
/// </summary>
[Serializable]
public class AssetBookableDateDto
{
    public DateTime Date { get; set; }

    /// <summary>
    /// Periods can be booked in this date.
    /// </summary>
    public List<AssetBookablePeriodDto> BookablePeriods { get; set; } = new();
}