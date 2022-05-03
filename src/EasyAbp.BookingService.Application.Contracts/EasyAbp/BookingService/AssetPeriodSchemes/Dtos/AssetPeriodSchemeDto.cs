using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetPeriodSchemes.Dtos;

[Serializable]
public class AssetPeriodSchemeDto : ExtensibleAuditedEntityDto<AssetPeriodSchemeKey>
{
    public Guid PeriodSchemeId { get; set; }
}