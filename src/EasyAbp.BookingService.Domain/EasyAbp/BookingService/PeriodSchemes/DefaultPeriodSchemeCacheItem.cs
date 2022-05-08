using System;

namespace EasyAbp.BookingService.PeriodSchemes;

[Serializable]
public class DefaultPeriodSchemeCacheItem
{
    public const string Key = "DefaultPeriodScheme";

    public PeriodScheme Value { get; set; }
}