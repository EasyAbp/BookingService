using System;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetPeriodSchemes.Dtos;

[Serializable]
public class UpdateAssetPeriodSchemeDto : ExtensibleObject
{
    public DateTime Date { get; set; }
}