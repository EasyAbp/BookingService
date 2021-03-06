using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetOccupancies;

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
    [Route("search-booking-periods")]
    public virtual Task<SearchBookingPeriodsResultDto> SearchBookingPeriodsAsync(SearchBookingPeriodsInputDto input)
    {
        return _service.SearchBookingPeriodsAsync(input);
    }

    [HttpPost]
    [Route("search-category-booking-periods")]
    public virtual Task<SearchBookingPeriodsResultDto> SearchCategoryBookingPeriodsAsync(
        SearchCategoryBookingPeriodsInputDto input)
    {
        return _service.SearchCategoryBookingPeriodsAsync(input);
    }

    [HttpPost]
    [Route("check-create")]
    public virtual Task CheckCreateAsync(CreateAssetOccupancyDto input)
    {
        return _service.CheckCreateAsync(input);
    }

    [HttpPost]
    [Route("check-create-by-category-id")]
    public virtual Task CheckCreateByCategoryIdAsync(
        CreateAssetOccupancyByCategoryIdDto input)
    {
        return _service.CheckCreateByCategoryIdAsync(input);
    }

    [HttpPost]
    [Route("check-bulk-create")]
    public virtual Task CheckBulkCreateAsync(BulkCreateAssetOccupancyDto input)
    {
        return _service.CheckBulkCreateAsync(input);
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