using System;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

[Serializable]
public class AssetPeriodSchemeKey
{
    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }
}