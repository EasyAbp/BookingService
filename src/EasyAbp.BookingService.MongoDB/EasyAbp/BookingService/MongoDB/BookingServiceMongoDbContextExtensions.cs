using Volo.Abp;
using Volo.Abp.MongoDB;

namespace EasyAbp.BookingService.MongoDB;

public static class BookingServiceMongoDbContextExtensions
{
    public static void ConfigureBookingService(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
