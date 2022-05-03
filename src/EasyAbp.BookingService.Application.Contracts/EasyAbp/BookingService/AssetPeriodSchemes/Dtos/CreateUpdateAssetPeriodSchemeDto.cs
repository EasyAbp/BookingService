using System;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetPeriodSchemes.Dtos;

[Serializable]
public class CreateUpdateAssetPeriodSchemeDto : ExtensibleObject
{
    public Guid PeriodSchemeId { get; set; }
}