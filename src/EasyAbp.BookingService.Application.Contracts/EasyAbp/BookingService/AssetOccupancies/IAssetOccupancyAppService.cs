using System;
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

}