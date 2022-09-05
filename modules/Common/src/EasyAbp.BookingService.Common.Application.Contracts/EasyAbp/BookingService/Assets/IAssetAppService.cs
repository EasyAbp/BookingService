using System;
using EasyAbp.BookingService.Assets.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.Assets;

public interface IAssetAppService :
    ICrudAppService<
        AssetDto,
        Guid,
        GetAssetsRequestDto,
        CreateUpdateAssetDto,
        CreateUpdateAssetDto>
{
}