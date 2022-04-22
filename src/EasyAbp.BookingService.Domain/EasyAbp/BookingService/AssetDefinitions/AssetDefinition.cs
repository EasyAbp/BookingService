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
    public virtual AssetSchedulePolicy DefaultSchedulePolicy { get; }
    
    /// <summary>
    /// How many days in advance can you occupy the asset.
    /// The property value from <see cref="AssetCategory"/> is preferred.
    /// </summary>
    public virtual int DaysInAdvance { get; }

    public AssetDefinition([NotNull] string name, AssetSchedulePolicy defaultSchedulePolicy, int daysInAdvance)
    {
        Name = name;
        DefaultSchedulePolicy = defaultSchedulePolicy;
        DaysInAdvance = daysInAdvance;
    }
}