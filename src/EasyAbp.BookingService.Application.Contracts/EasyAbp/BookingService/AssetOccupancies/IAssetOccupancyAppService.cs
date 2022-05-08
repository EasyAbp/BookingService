using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetOccupancies;

public interface IAssetOccupancyAppService :
    ICrudAppService<
        AssetOccupancyDto,
        Guid,
        GetAssetOccupanciesRequestDto,
        CreateUpdateAssetOccupancyDto,
        CreateUpdateAssetOccupancyDto>
{
    /// <summary>
    /// Get asset's bookable dates in range of days
    /// </summary>
    Task<List<AssetBookableDateDto>> SearchAssetBookableDatesAsync(SearchAssetBookableDatesRequestDto input);

    /// <summary>
    /// Search for asset's bookable dates in all categories recursively
    /// </summary>
    Task<List<CategoryBookableDateDto>> SearchCategoryBookableDatesAsync(SearchCategoryBookableDatesRequestDto input);
}