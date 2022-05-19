using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetSchedules;
using JetBrains.Annotations;

namespace EasyAbp.BookingService.AssetDefinitions;

public class AssetDefinition
{
    [NotNull]
    public string Name { get; }
    
    /// <summary>
    /// This property determines whether assets can be occupied by default when there is no schedule created.
    /// The property value from <see cref="AssetCategory"/> is preferred.
    /// </summary>
    public virtual PeriodUsable DefaultPeriodUsable { get; }
    
    /// <summary>
    /// This value object describes the time range for assets that can occupy.
    /// The property value from <see cref="AssetCategory"/> is preferred.
    /// </summary>
    [NotNull]
    public virtual TimeInAdvance TimeInAdvance { get; }

    public AssetDefinition([NotNull] string name, PeriodUsable defaultPeriodUsable,
        [NotNull] TimeInAdvance timeInAdvance)
    {
        Name = name;
        DefaultPeriodUsable = defaultPeriodUsable;
        TimeInAdvance = timeInAdvance;
    }
}