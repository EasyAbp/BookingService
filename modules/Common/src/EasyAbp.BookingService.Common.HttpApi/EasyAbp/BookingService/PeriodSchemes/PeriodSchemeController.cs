using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.PeriodSchemes;

[RemoteService(Name = BookingServiceRemoteServiceConsts.RemoteServiceName)]
[Route("/api/booking-service/period-scheme")]
public class PeriodSchemeController : BookingServiceController, IPeriodSchemeAppService
{
    private readonly IPeriodSchemeAppService _service;

    public PeriodSchemeController(IPeriodSchemeAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("{id}/set-as-default")]
    public virtual Task<PeriodSchemeDto> SetAsDefaultAsync(Guid id)
    {
        return _service.SetAsDefaultAsync(id);
    }

    [HttpPost]
    [Route("")]
    public virtual Task<PeriodSchemeDto> CreateAsync(CreatePeriodSchemeDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<PeriodSchemeDto> UpdateAsync(Guid id, UpdatePeriodSchemeDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<PeriodSchemeDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
    public virtual Task<PagedResultDto<PeriodSchemeDto>> GetListAsync(GetPeriodSchemesRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPost]
    [Route("{periodSchemeId}/period")]
    public virtual Task<PeriodSchemeDto> CreatePeriodAsync(Guid periodSchemeId, CreateUpdatePeriodDto input)
    {
        return _service.CreatePeriodAsync(periodSchemeId, input);
    }

    [HttpPut]
    [Route("{periodSchemeId}/period/{periodId}")]
    public virtual Task<PeriodSchemeDto> UpdatePeriodAsync(Guid periodSchemeId, Guid periodId,
        CreateUpdatePeriodDto input)
    {
        return _service.UpdatePeriodAsync(periodSchemeId, periodId, input);
    }

    [HttpDelete]
    [Route("{periodSchemeId}/period/{periodId}")]
    public virtual Task<PeriodSchemeDto> DeletePeriodAsync(Guid periodSchemeId, Guid periodId)
    {
        return _service.DeletePeriodAsync(periodSchemeId, periodId);
    }
}