using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetOccupancies;

public interface IAssetOccupancyAppService :
    ICrudAppService<
        AssetOccupancyDto,
        Guid,
        GetAssetOccupanciesRequestDto,
        CreateAssetOccupancyDto>
{
    Task<AssetOccupancyDto> CreateByCategoryIdAsync(CreateAssetOccupancyByCategoryIdDto input);

    /// <summary>
    /// Search for asset's bookable periods on specific date
    /// </summary>
    Task<SearchBookablePeriodResultDto> SearchAssetBookablePeriodsAsync(SearchAssetBookablePeriodsRequestDto input);

    /// <summary>
    /// Search for category's bookable periods on specific date
    /// </summary>
    Task<SearchBookablePeriodResultDto> SearchCategoryBookablePeriodsAsync(SearchCategoryBookablePeriodsRequestDto input);

    Task CheckCreateAsync(CreateAssetOccupancyDto input);

    Task CheckCreateByCategoryIdAsync(CreateAssetOccupancyByCategoryIdDto input);
    
    Task CheckBulkCreateAsync(BulkCreateAssetOccupancyDto input);
}