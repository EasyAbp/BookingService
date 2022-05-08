using System;
using System.Collections.Generic;

namespace EasyAbp.BookingService.AssetOccupancies;

[Serializable]
public class AssetBookableDate
{
    public DateTime Date { get; set; }

    public List<AssetBookablePeriod> BookablePeriods { get; set; } = new();
}