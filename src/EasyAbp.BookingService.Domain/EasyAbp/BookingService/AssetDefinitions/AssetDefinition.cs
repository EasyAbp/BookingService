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
    /// The SchedulePolicy of <see cref="AssetCategory"/> is preferred.
    /// </summary>
    public virtual AssetSchedulePolicy DefaultSchedulePolicy { get; }

    public AssetDefinition([NotNull] string name, AssetSchedulePolicy defaultSchedulePolicy)
    {
        Name = name;
        DefaultSchedulePolicy = defaultSchedulePolicy;
    }
}