using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.EntityFrameworkCore;

public class BookingServiceHttpApiHostMigrationsDbContext : AbpDbContext<BookingServiceHttpApiHostMigrationsDbContext>
{
    public BookingServiceHttpApiHostMigrationsDbContext(DbContextOptions<BookingServiceHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureBookingService();
    }
}
