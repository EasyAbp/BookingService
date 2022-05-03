using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetCategories;
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

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "Questions", BookingServiceDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        // TODO Add Entity indexes

        builder.Entity<AssetCategory>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "AssetCategories",
                BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.OwnsOne(x => x.TimeInAdvance);

            b.HasIndex(x => x.AssetDefinitionName);
            b.HasIndex(x => x.Disabled);
            /* Configure more properties here */
        });


        builder.Entity<AssetOccupancy>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "AssetOccupancies",
                BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.HasIndex(x => x.AssetId);

            /* Configure more properties here */
        });


        builder.Entity<AssetPeriodScheme>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "AssetPeriodSchemes",
                BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
        });


        builder.Entity<Asset>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "Assets", BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.OwnsOne(x => x.TimeInAdvance);

            b.HasIndex(x => x.AssetDefinitionName);
            b.HasIndex(x => x.Disabled);
            b.HasIndex(x => x.AssetCategoryId);

            /* Configure more properties here */
        });


        builder.Entity<AssetSchedule>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "AssetSchedules", BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.OwnsOne(x => x.TimeInAdvance);

            b.HasIndex(x => x.AssetId);
            b.HasIndex(x => new { x.AssetId, x.Date, x.StartingTime }).IsUnique();

            /* Configure more properties here */
        });


        builder.Entity<PeriodScheme>(b =>
        {
            b.ToTable(BookingServiceDbProperties.DbTablePrefix + "PeriodSchemes", BookingServiceDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.HasIndex(x => x.IsDefault);
            /* Configure more properties here */
        });
    }
}