using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class GetPeriodSchemesRequestDto : PagedAndSortedResultRequestDto
{
    public string Name { get; set; }
}