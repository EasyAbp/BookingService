using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace EasyAbp.BookingService.MongoDB;

[DependsOn(
    typeof(BookingServiceDomainModule),
    typeof(AbpMongoDbModule),
    typeof(BookingServiceCommonMongoDbModule)
)]
public class BookingServiceMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<BookingServiceMongoDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, MongoQuestionRepository>();
             */
        });
    }
}