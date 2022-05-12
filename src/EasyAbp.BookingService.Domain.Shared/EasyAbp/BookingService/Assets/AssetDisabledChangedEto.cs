using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.Assets;

[Serializable]
public class AssetDisabledChangedEto : IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid AssetId { get; set; }

    /// <summary>
    /// The value Asset has changed to.
    /// </summary>
    public bool Disabled { get; set; }
}