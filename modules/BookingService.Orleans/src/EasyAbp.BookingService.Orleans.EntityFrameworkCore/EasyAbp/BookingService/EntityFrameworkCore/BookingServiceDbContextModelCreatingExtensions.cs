using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace EasyAbp.BookingService.EntityFrameworkCore;

public static class BookingServiceDbContextModelCreatingExtensions
{
    public static void ConfigureBookingService(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureBookingServiceCommon();
    }
}