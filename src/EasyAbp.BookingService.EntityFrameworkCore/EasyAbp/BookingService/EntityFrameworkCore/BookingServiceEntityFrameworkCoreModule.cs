using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[DependsOn(
    typeof(BookingServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class BookingServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BookingServiceDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
        });
    }
}
