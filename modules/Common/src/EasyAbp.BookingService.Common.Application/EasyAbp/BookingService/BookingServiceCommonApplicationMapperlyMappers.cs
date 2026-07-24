using System.Collections.Generic;
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
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace EasyAbp.BookingService;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TimeInAdvanceMapper : TwoWayMapperBase<TimeInAdvance, TimeInAdvanceDto>
{
    public override partial TimeInAdvanceDto Map(TimeInAdvance source);
    public override partial void Map(TimeInAdvance source, TimeInAdvanceDto destination);
    public override partial TimeInAdvance ReverseMap(TimeInAdvanceDto destination);
    public override partial void ReverseMap(TimeInAdvanceDto destination, TimeInAdvance source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class AssetCategoryMapper : MapperBase<AssetCategory, AssetCategoryDto>
{
    public override partial AssetCategoryDto Map(AssetCategory source);
    public override partial void Map(AssetCategory source, AssetCategoryDto destination);

    // AutoMapper maps a null source collection to an empty destination collection by default.
    public override void AfterMap(AssetCategory source, AssetCategoryDto destination)
    {
        destination.Children ??= new List<AssetCategoryDto>();
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class AssetOccupancyMapper : MapperBase<AssetOccupancy, AssetOccupancyDto>
{
    public override partial AssetOccupancyDto Map(AssetOccupancy source);
    public override partial void Map(AssetOccupancy source, AssetOccupancyDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class AssetPeriodSchemeMapper : MapperBase<AssetPeriodScheme, AssetPeriodSchemeDto>
{
    public override partial AssetPeriodSchemeDto Map(AssetPeriodScheme source);
    public override partial void Map(AssetPeriodScheme source, AssetPeriodSchemeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class AssetMapper : MapperBase<Asset, AssetDto>
{
    public override partial AssetDto Map(Asset source);
    public override partial void Map(Asset source, AssetDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class PeriodSchemeMapper : MapperBase<PeriodScheme, PeriodSchemeDto>
{
    public override partial PeriodSchemeDto Map(PeriodScheme source);
    public override partial void Map(PeriodScheme source, PeriodSchemeDto destination);

    // AutoMapper maps a null source collection to an empty destination collection by default.
    public override void AfterMap(PeriodScheme source, PeriodSchemeDto destination)
    {
        destination.Periods ??= new List<PeriodDto>();
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PeriodMapper : MapperBase<Period, PeriodDto>
{
    public override partial PeriodDto Map(Period source);
    public override partial void Map(Period source, PeriodDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BookingPeriodMapper : MapperBase<PeriodOccupancyModel, BookingPeriodDto>
{
    public override partial BookingPeriodDto Map(PeriodOccupancyModel source);
    public override partial void Map(PeriodOccupancyModel source, BookingPeriodDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class AssetScheduleMapper : MapperBase<AssetSchedule, AssetScheduleDto>
{
    public override partial AssetScheduleDto Map(AssetSchedule source);
    public override partial void Map(AssetSchedule source, AssetScheduleDto destination);
}
