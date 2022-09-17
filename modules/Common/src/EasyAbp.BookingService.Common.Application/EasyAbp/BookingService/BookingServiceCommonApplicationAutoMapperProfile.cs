using AutoMapper;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetCategories.Dtos;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.AssetOccupancyProviders;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.Assets.Dtos;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using EasyAbp.BookingService.Dtos;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.PeriodSchemes.Dtos;

namespace EasyAbp.BookingService;

public class BookingServiceCommonApplicationAutoMapperProfile : Profile
{
    public BookingServiceCommonApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<TimeInAdvance, TimeInAdvanceDto>();
        CreateMap<TimeInAdvanceDto, TimeInAdvance>();

        CreateMap<AssetCategory, AssetCategoryDto>();
        CreateMap<AssetOccupancy, AssetOccupancyDto>();
        CreateMap<AssetPeriodScheme, AssetPeriodSchemeDto>();
        CreateMap<Asset, AssetDto>();
        CreateMap<PeriodScheme, PeriodSchemeDto>();
        CreateMap<Period, PeriodDto>();
        CreateMap<PeriodOccupancyModel, BookingPeriodDto>();

        CreateMap<AssetSchedule, AssetScheduleDto>();

        CreateMap<CreateAssetPeriodSchemeDto, AssetPeriodScheme>(MemberList.Source);
        CreateMap<UpdateAssetPeriodSchemeDto, AssetPeriodScheme>(MemberList.Source);
    }
}