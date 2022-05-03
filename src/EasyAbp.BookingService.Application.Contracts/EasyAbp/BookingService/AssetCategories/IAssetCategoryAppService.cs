using System;
using EasyAbp.BookingService.AssetCategories.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetCategories;

public interface IAssetCategoryAppService :
    ICrudAppService<
        AssetCategoryDto,
        Guid,
        GetAssetCategoriesRequestDto,
        CreateUpdateAssetCategoryDto,
        CreateUpdateAssetCategoryDto>
{
}