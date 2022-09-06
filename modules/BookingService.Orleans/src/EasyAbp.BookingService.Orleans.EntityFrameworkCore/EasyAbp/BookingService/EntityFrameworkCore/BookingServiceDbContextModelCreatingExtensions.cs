using EasyAbp.BookingService.AssetSchedules;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.BookingService.EntityFrameworkCore;

public static class BookingServiceDbContextModelCreatingExtensions
{
    public static void ConfigureBookingService(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureBookingServiceCommon();

        builder.Entity<AssetSchedule>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "AssetSchedules", BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.OwnsOne(x => x.TimeInAdvance);
            b.HasIndex(x => new { x.Date, x.AssetId, x.PeriodSchemeId });
        });
    }
}