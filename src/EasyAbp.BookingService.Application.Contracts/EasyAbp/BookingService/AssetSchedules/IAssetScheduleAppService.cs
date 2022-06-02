using System;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleAppService :
    ICrudAppService<
        AssetScheduleDto,
        Guid,
        GetAssetSchedulesRequestDto,
        CreateAssetScheduleDto,
        UpdateAssetScheduleDto>
{
}