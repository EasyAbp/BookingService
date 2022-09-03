using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetOccupancyCounts;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.PeriodSchemes;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[ConnectionStringName(BookingServiceDbProperties.ConnectionStringName)]
public interface IBookingServiceDbContext : IBookingServiceCommonDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    DbSet<AssetSchedule> AssetSchedules { get; set; }
    DbSet<AssetOccupancyCount> AssetOccupancyCounts { get; set; }
}