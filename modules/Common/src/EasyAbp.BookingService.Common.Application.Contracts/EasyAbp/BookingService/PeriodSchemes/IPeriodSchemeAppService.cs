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
        CreatePeriodSchemeDto,
        UpdatePeriodSchemeDto>
{
    /// <summary>
    /// This method will change the default Period Scheme entity by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PeriodSchemeDto> SetAsDefaultAsync(Guid id);
    
    Task<PeriodSchemeDto> CreatePeriodAsync(Guid periodSchemeId, CreateUpdatePeriodDto input);

    Task<PeriodSchemeDto> UpdatePeriodAsync(Guid periodSchemeId, Guid periodId, CreateUpdatePeriodDto input);

    Task<PeriodSchemeDto> DeletePeriodAsync(Guid periodSchemeId, Guid periodId);
}