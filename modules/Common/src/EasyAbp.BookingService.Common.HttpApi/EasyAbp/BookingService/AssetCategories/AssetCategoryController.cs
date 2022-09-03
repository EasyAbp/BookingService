using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetCategories;

[RemoteService(Name = BookingServiceRemoteServiceConsts.RemoteServiceName)]
[Route("/api/booking-service/asset-category")]
public class AssetCategoryController : BookingServiceController, IAssetCategoryAppService
{
    private readonly IAssetCategoryAppService _service;

    public AssetCategoryController(IAssetCategoryAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("")]
    public virtual Task<AssetCategoryDto> CreateAsync(CreateAssetCategoryDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<AssetCategoryDto> UpdateAsync(Guid id, UpdateAssetCategoryDto input)
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
    public virtual Task<AssetCategoryDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
    public virtual Task<PagedResultDto<AssetCategoryDto>> GetListAsync(GetAssetCategoriesRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}