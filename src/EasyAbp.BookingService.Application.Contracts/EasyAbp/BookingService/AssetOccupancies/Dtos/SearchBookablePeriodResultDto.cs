using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class SearchBookablePeriodResultDto : ListResultDto<BookablePeriodDto>
{
    public SearchBookablePeriodResultDto()
    {
    }
    
    public SearchBookablePeriodResultDto(IReadOnlyList<BookablePeriodDto> items) : base(items)
    {
    }
}