using Volo.Abp.Guids;

namespace EasyAbp.BookingService.EntityFrameworkCore;

/* This class can be used as a base class for EF Core integration tests,
 * while SampleRepository_Tests uses a different approach.
 */
public abstract class
    BookingServiceCommonEntityFrameworkCoreTestBase : BookingServiceTestBase<BookingServiceCommonEntityFrameworkCoreTestModule>
{
    protected readonly IGuidGenerator GuidGenerator;

    protected BookingServiceCommonEntityFrameworkCoreTestBase()
    {
        GuidGenerator = GetRequiredService<IGuidGenerator>();
    }
}