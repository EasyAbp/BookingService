using EasyAbp.BookingService.AssetCategories.Dtos;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Assets.Dtos;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using AutoMapper;
using EasyAbp.BookingService.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetCategories.AssetCategory.ViewModels;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetOccupancies.AssetOccupancy.ViewModels;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetPeriodSchemes.AssetPeriodScheme.ViewModels;
using EasyAbp.BookingService.Web.Pages.BookingService.Assets.Asset.ViewModels;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetSchedules.AssetSchedule.ViewModels;
using EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.Period.ViewModels;
using EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.PeriodScheme.ViewModels;
using EasyAbp.BookingService.Web.Pages.BookingService.ViewModels;

namespace EasyAbp.BookingService.Web;

public class BookingServiceCommonWebAutoMapperProfile : Profile
{
    public BookingServiceCommonWebAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<TimeInAdvanceDto, TimeInAdvanceViewModel>();
        CreateMap<TimeInAdvanceViewModel, TimeInAdvanceDto>();

        CreateMap<AssetCategoryDto, EditAssetCategoryViewModel>();
        CreateMap<CreateAssetCategoryViewModel, CreateAssetCategoryDto>();
        CreateMap<EditAssetCategoryViewModel, UpdateAssetCategoryDto>();

        CreateMap<CreateAssetOccupancyViewModel, CreateAssetOccupancyDto>();

        CreateMap<AssetPeriodSchemeDto, EditAssetPeriodSchemeViewModel>();
        CreateMap<CreateAssetPeriodSchemeViewModel, CreateAssetPeriodSchemeDto>();
        CreateMap<EditAssetPeriodSchemeViewModel, UpdateAssetPeriodSchemeDto>();

        CreateMap<AssetDto, CreateEditAssetViewModel>();
        CreateMap<CreateEditAssetViewModel, CreateUpdateAssetDto>();

        CreateMap<AssetScheduleDto, EditAssetScheduleViewModel>();
        CreateMap<CreateAssetScheduleViewModel, CreateAssetScheduleDto>();
        CreateMap<EditAssetScheduleViewModel, UpdateAssetScheduleDto>();

        CreateMap<PeriodSchemeDto, CreateEditPeriodSchemeViewModel>(MemberList.Destination);
        CreateMap<CreateEditPeriodSchemeViewModel, CreatePeriodSchemeDto>(MemberList.Source);
        CreateMap<CreateEditPeriodSchemeViewModel, UpdatePeriodSchemeDto>(MemberList.Source);
        CreateMap<CreateEditPeriodViewModel, CreateUpdatePeriodDto>();
    }
}