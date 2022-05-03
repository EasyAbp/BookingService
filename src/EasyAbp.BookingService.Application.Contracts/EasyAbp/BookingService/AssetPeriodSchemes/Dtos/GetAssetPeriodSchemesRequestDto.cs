using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetPeriodSchemes.Dtos;

[Serializable]
public class GetAssetPeriodSchemesRequestDto : PagedAndSortedResultRequestDto
{
    public Guid? PeriodSchemeId { get; set; }
}