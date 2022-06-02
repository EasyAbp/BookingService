using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public interface IAssetPeriodSchemeAppService :
    ICrudAppService< 
        AssetPeriodSchemeDto, 
        AssetPeriodSchemeKey, 
        GetAssetPeriodSchemesRequestDto,
        CreateUpdateAssetPeriodSchemeDto,
        CreateUpdateAssetPeriodSchemeDto>
{

}