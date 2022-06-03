using System;
using System.Threading.Tasks;
using EasyAbp.BookingService;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace PeriodSchemes;

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
    public virtual Task<PeriodSchemeDto> CreateAsync(CreateUpdatePeriodSchemeDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<PeriodSchemeDto> UpdateAsync(Guid id, CreateUpdatePeriodSchemeDto input)
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
}