using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[ConnectionStringName(BookingServiceDbProperties.ConnectionStringName)]
public interface IBookingServiceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
