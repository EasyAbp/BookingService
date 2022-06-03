using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace AssetOccupancies;

[RemoteService(Name = BookingServiceRemoteServiceConsts.RemoteServiceName)]
[Route("/api/booking-service/asset-occupancy")]
public class AssetOccupancyController : BookingServiceController, IAssetOccupancyAppService
{
    private readonly IAssetOccupancyAppService _service;

    public AssetOccupancyController(IAssetOccupancyAppService service)
    {
        _service = service;
    }

    [RemoteService(false)]
    [HttpPut]
    [Route("{id}")]
    public virtual Task<AssetOccupancyDto> UpdateAsync(Guid id, CreateAssetOccupancyDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpPost]
    [Route("")]
    public virtual Task<AssetOccupancyDto> CreateAsync(CreateAssetOccupancyDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPost]
    [Route("by-category-id")]
    public virtual Task<AssetOccupancyDto> CreateByCategoryIdAsync(CreateAssetOccupancyByCategoryIdDto input)
    {
        return _service.CreateByCategoryIdAsync(input);
    }

    [HttpPost]
    [Route("search-asset-bookable-periods")]
    public virtual Task<List<BookablePeriodDto>> SearchAssetBookablePeriodsAsync(SearchAssetBookablePeriodsRequestDto input)
    {
        return _service.SearchAssetBookablePeriodsAsync(input);
    }

    [HttpPost]
    [Route("search-category-bookable-periods")]
    public virtual Task<List<BookablePeriodDto>> SearchCategoryBookablePeriodsAsync(SearchCategoryBookablePeriodsRequestDto input)
    {
        return _service.SearchCategoryBookablePeriodsAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<AssetOccupancyDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
    public virtual Task<PagedResultDto<AssetOccupancyDto>> GetListAsync(GetAssetOccupanciesRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}