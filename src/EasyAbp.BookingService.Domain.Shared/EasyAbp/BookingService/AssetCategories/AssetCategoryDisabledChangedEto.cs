using System;

namespace EasyAbp.BookingService.AssetCategories;

[Serializable]
public class AssetCategoryDisabledChangedEto
{
    public Guid AssetCategoryId { get; set; }

    /// <summary>
    /// The value Asset Category has changed to.
    /// </summary>
    public bool Disabled { get; set; }
}