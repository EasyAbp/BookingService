using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.PeriodSchemes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.BookingService.EntityFrameworkCore;

public static class BookingServiceCommonDbContextModelCreatingExtensions
{
    public static void ConfigureBookingServiceCommon(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<AssetCategory>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "AssetCategories",
                BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.OwnsOne(x => x.TimeInAdvance);
        });

        builder.Entity<AssetOccupancy>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "AssetOccupancies",
                BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.HasIndex(x => new { x.Date, x.AssetId, x.StartingTime, x.Duration });
            b.HasIndex(x => new { x.Date, x.OccupierUserId });
        });

        builder.Entity<AssetPeriodScheme>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "AssetPeriodSchemes",
                BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.HasKey(x => new { x.Date, x.AssetId });
        });

        builder.Entity<Asset>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "Assets", BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.OwnsOne(x => x.TimeInAdvance);
        });

        builder.Entity<PeriodScheme>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "PeriodSchemes", BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
        });

        builder.Entity<Period>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "Periods", BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
        });
    }
}