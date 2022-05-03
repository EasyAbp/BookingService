using System;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public interface IPeriodSchemeAppService :
    ICrudAppService<
        PeriodSchemeDto,
        Guid,
        GetPeriodSchemesRequestDto,
        CreatePeriodSchemeDto,
        UpdatePeriodSchemeDto>
{
}