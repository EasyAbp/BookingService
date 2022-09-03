using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class SearchBookingPeriodsResultDto : ListResultDto<BookingPeriodDto>
{
    public SearchBookingPeriodsResultDto()
    {
    }
    
    public SearchBookingPeriodsResultDto(IReadOnlyList<BookingPeriodDto> items) : base(items)
    {
    }
}