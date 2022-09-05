using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.BookingService.MongoDB;

[ConnectionStringName(BookingServiceDbProperties.ConnectionStringName)]
public interface IBookingServiceCommonMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
