using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.EntityFrameworkCore;
using JetBrains.Annotations;
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

        [ItemCanBeNull]
        public virtual Task<PeriodScheme> FindDefaultSchemeAsync()
        {
            return FindAsync(x => x.IsDefault);
        }
    }
}