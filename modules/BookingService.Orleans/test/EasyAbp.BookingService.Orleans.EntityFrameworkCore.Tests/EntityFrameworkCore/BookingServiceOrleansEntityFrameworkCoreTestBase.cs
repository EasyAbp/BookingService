using Volo.Abp.Guids;

namespace EasyAbp.BookingService.EntityFrameworkCore;

/* This class can be used as a base class for EF Core integration tests,
 * while SampleRepository_Tests uses a different approach.
 */
public abstract class
    BookingServiceOrleansEntityFrameworkCoreTestBase : BookingServiceTestBase<BookingServiceOrleansEntityFrameworkCoreTestModule>
{
    protected readonly IGuidGenerator GuidGenerator;

    protected BookingServiceOrleansEntityFrameworkCoreTestBase()
    {
        GuidGenerator = GetRequiredService<IGuidGenerator>();
    }
}