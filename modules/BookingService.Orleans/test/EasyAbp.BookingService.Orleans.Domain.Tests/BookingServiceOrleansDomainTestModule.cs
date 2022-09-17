using EasyAbp.BookingService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(BookingServiceOrleansEntityFrameworkCoreTestModule)
    )]
public class BookingServiceOrleansDomainTestModule : AbpModule
{

}
