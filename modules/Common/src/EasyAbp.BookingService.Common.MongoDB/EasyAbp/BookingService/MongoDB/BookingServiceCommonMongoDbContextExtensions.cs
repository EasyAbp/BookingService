using Volo.Abp;
using Volo.Abp.MongoDB;

namespace EasyAbp.BookingService.MongoDB;

public static class BookingServiceCommonMongoDbContextExtensions
{
    public static void ConfigureBookingServiceCommon(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
