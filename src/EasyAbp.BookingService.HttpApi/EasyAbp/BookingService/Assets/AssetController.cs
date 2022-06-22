using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.Assets.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.Assets;

[RemoteService(Name = BookingServiceRemoteServiceConsts.RemoteServiceName)]
[Route("/api/booking-service/asset")]
public class AssetController : BookingServiceController, IAssetAppService
{
    private readonly IAssetAppService _service;

    public AssetController(IAssetAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("")]
    public virtual Task<AssetDto> CreateAsync(CreateUpdateAssetDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<AssetDto> UpdateAsync(Guid id, CreateUpdateAssetDto input)
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
    public virtual Task<AssetDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
    public virtual Task<PagedResultDto<AssetDto>> GetListAsync(GetAssetsRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}