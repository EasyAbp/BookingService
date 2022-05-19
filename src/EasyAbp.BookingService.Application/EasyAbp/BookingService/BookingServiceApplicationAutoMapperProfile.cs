using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetCategories.Dtos;
using AutoMapper;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.Assets.Dtos;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using EasyAbp.BookingService.Dtos;

namespace EasyAbp.BookingService;

public class BookingServiceApplicationAutoMapperProfile : Profile
{
    public BookingServiceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<TimeInAdvance, TimeInAdvanceDto>();
        CreateMap<TimeInAdvanceDto, TimeInAdvance>();

        CreateMap<AssetCategory, AssetCategoryDto>();
        CreateMap<CreateAssetCategoryDto, AssetCategory>(MemberList.Source);
        CreateMap<UpdateAssetCategoryDto, AssetCategory>(MemberList.Source);
        CreateMap<AssetOccupancy, AssetOccupancyDto>();
        CreateMap<CreateAssetOccupancyDto, AssetOccupancy>(MemberList.Source);
        CreateMap<AssetPeriodScheme, AssetPeriodSchemeDto>();
        CreateMap<CreateUpdateAssetPeriodSchemeDto, AssetPeriodScheme>(MemberList.Source);
        CreateMap<Asset, AssetDto>();
        CreateMap<CreateUpdateAssetDto, Asset>(MemberList.Source);
        CreateMap<AssetSchedule, AssetScheduleDto>();
        CreateMap<CreateUpdateAssetScheduleDto, AssetSchedule>(MemberList.Source);
        CreateMap<PeriodScheme, PeriodSchemeDto>();
        CreateMap<CreateUpdatePeriodSchemeDto, PeriodScheme>(MemberList.Source);
        CreateMap<Period, PeriodDto>();
        CreateMap<BookablePeriod, BookablePeriodDto>();
        CreateMap<CreatePeriodDto, Period>(MemberList.Source);
    }
}