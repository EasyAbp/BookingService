using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[ConnectionStringName(BookingServiceDbProperties.ConnectionStringName)]
public class BookingServiceDbContext : AbpDbContext<BookingServiceDbContext>, IBookingServiceDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

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
