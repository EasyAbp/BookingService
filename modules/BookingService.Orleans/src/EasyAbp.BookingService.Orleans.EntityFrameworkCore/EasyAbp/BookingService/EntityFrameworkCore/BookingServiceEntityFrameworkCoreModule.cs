using EasyAbp.Abp.Trees.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.BookingService.EntityFrameworkCore;

[DependsOn(
    typeof(BookingServiceOrleansDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpTreesEntityFrameworkCoreModule),
    typeof(BookingServiceCommonEntityFrameworkCoreModule)
)]
public class BookingServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BookingServiceDbContext>(options =>
        {
            BookingServiceCommonEntityFrameworkCoreModule.AddCommonRepositories(options);

            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });
    }
}