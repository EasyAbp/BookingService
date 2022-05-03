using System;

namespace EasyAbp.BookingService.Assets;

[Serializable]
public class AssetDisabledChangedEto
{
    public Guid AssetId { get; set; }

    /// <summary>
    /// The value Asset has changed to.
    /// </summary>
    public bool Disabled { get; set; }
}