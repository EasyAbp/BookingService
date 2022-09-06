using EasyAbp.BookingService.AssetSchedules;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[ConnectionStringName(BookingServiceDbProperties.ConnectionStringName)]
public interface IBookingServiceDbContext : IBookingServiceCommonDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    DbSet<AssetSchedule> AssetSchedules { get; set; }
}