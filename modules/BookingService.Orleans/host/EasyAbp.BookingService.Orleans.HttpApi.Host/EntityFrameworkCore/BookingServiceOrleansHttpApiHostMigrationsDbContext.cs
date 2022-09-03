using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.EntityFrameworkCore;

public class BookingServiceOrleansHttpApiHostMigrationsDbContext : AbpDbContext<BookingServiceOrleansHttpApiHostMigrationsDbContext>
{
    public BookingServiceOrleansHttpApiHostMigrationsDbContext(DbContextOptions<BookingServiceOrleansHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureBookingService();
    }
}
