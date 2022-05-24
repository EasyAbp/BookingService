using System;

namespace EasyAbp.BookingService.PeriodSchemes;

[Serializable]
public class DefaultPeriodSchemeCacheItem
{
    public const string Key = "DefaultPeriodScheme";

    public Guid PeriodSchemeId { get; set; }
}