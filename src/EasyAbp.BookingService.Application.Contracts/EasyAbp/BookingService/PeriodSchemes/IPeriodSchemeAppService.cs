using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public interface IPeriodSchemeAppService :
    ICrudAppService<
        PeriodSchemeDto,
        Guid,
        GetPeriodSchemesRequestDto,
        CreateUpdatePeriodSchemeDto,
        CreateUpdatePeriodSchemeDto>
{
    /// <summary>
    /// This method will change the default Period Scheme entity by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PeriodSchemeDto> SetAsDefaultAsync(Guid id);
}