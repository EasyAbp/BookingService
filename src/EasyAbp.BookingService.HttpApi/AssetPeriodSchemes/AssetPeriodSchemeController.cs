using System.Threading.Tasks;
using EasyAbp.BookingService;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace AssetPeriodSchemes;

[RemoteService(Name = BookingServiceRemoteServiceConsts.RemoteServiceName)]
[Route("/api/booking-service/asset-period-scheme")]
public class AssetPeriodSchemeController : BookingServiceController, IAssetPeriodSchemeAppService
{
    private readonly IAssetPeriodSchemeAppService _service;

    public AssetPeriodSchemeController(IAssetPeriodSchemeAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("")]
    public virtual Task<AssetPeriodSchemeDto> CreateAsync(CreateUpdateAssetPeriodSchemeDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{AssetId}/{Date}")]
    public virtual Task<AssetPeriodSchemeDto> UpdateAsync(AssetPeriodSchemeKey id, CreateUpdateAssetPeriodSchemeDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{AssetId}/{Date}")]
    public virtual Task DeleteAsync(AssetPeriodSchemeKey id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{AssetId}/{Date}")]
    public virtual Task<AssetPeriodSchemeDto> GetAsync(AssetPeriodSchemeKey id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
    public virtual Task<PagedResultDto<AssetPeriodSchemeDto>> GetListAsync(GetAssetPeriodSchemesRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}