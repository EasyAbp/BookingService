using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.BookingService.PeriodSchemes
{
    public class PeriodSchemeRepository : EfCoreRepository<IBookingServiceDbContext, PeriodScheme, Guid>,
        IPeriodSchemeRepository
    {
        public PeriodSchemeRepository(IDbContextProvider<IBookingServiceDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public Task<PeriodScheme> GetDefaultSchemeAsync()
        {
            return FindAsync(x => x.IsDefault);
        }
    }
}