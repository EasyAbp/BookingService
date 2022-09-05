using System;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetPeriodSchemes.Dtos;

[Serializable]
public class CreateAssetPeriodSchemeDto : ExtensibleObject
{
    public Guid PeriodSchemeId { get; set; }

    public Guid AssetId { get; set; }

    public DateTime Date { get; set; }
}