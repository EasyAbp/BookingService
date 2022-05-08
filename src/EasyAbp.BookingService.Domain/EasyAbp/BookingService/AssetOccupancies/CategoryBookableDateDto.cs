using System;
using System.Collections.Generic;

namespace EasyAbp.BookingService.AssetOccupancies;

/// <summary>
/// Represent an category's bookable date.
/// </summary>
[Serializable]
public class CategoryBookableDate
{
    public DateTime Date { get; set; }

    /// <summary>
    /// Periods can be booked in this date.
    /// </summary>
    public List<CategoryBookablePeriod> BookablePeriods { get; set; } = new();
}