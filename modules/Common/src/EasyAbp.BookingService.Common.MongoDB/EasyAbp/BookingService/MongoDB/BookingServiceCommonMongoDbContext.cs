using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.BookingService.MongoDB;

[ConnectionStringName(BookingServiceDbProperties.ConnectionStringName)]
public class BookingServiceCommonMongoDbContext : AbpMongoDbContext, IBookingServiceCommonMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureBookingServiceCommon();
    }
}
