using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.PeriodSchemes;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[ConnectionStringName(BookingServiceDbProperties.ConnectionStringName)]
public interface IBookingServiceCommonDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    DbSet<AssetCategory> AssetCategories { get; set; }
    DbSet<AssetOccupancy> AssetOccupancies { get; set; }
    DbSet<AssetPeriodScheme> AssetPeriodSchemes { get; set; }
    DbSet<Asset> Assets { get; set; }
    DbSet<PeriodScheme> PeriodSchemes { get; set; }
    DbSet<Period> Periods { get; set; }
}