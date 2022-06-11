using System;
using System.Threading.Tasks;
using EasyAbp.BookingService;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace AssetSchedules;

[RemoteService(Name = BookingServiceRemoteServiceConsts.RemoteServiceName)]
[Route("/api/booking-service/asset-schedule")]
public class AssetScheduleController : BookingServiceController, IAssetScheduleAppService
{
    private readonly IAssetScheduleAppService _service;

    public AssetScheduleController(IAssetScheduleAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("")]
    public virtual Task<AssetScheduleDto> CreateAsync(CreateAssetScheduleDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<AssetScheduleDto> UpdateAsync(Guid id, UpdateAssetScheduleDto input)
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
    public virtual Task<AssetScheduleDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
    public virtual Task<PagedResultDto<AssetScheduleDto>> GetListAsync(GetAssetSchedulesRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}