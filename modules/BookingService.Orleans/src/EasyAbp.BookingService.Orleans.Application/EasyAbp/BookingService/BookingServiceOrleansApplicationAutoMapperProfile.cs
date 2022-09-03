using AutoMapper;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.AssetSchedules.Dtos;

namespace EasyAbp.BookingService;

public class BookingServiceOrleansApplicationAutoMapperProfile : Profile
{
    public BookingServiceOrleansApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<AssetSchedule, AssetScheduleDto>();
    }
}