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
public class BookingServiceDbContext : AbpDbContext<BookingServiceDbContext>, IBookingServiceDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */
    public DbSet<AssetCategory> AssetCategories { get; set; }
    public DbSet<AssetOccupancy> AssetOccupancies { get; set; }
    public DbSet<AssetPeriodScheme> AssetPeriodSchemes { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<AssetSchedule> AssetSchedules { get; set; }
    public DbSet<PeriodScheme> PeriodSchemes { get; set; }

    public BookingServiceDbContext(DbContextOptions<BookingServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureBookingService();
    }
}