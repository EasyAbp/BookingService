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
    /// Search for asset's booking periods on specific date
    /// </summary>
    Task<SearchBookingPeriodsResultDto> SearchBookingPeriodsAsync(SearchBookingPeriodsInputDto input);

    /// <summary>
    /// Search for category's booking periods on specific date
    /// </summary>
    Task<SearchBookingPeriodsResultDto> SearchCategoryBookingPeriodsAsync(SearchCategoryBookingPeriodsInputDto input);

    Task CheckCreateAsync(CreateAssetOccupancyDto input);

    Task CheckCreateByCategoryIdAsync(CreateAssetOccupancyByCategoryIdDto input);

    Task CheckBulkCreateAsync(BulkCreateAssetOccupancyDto input);
}