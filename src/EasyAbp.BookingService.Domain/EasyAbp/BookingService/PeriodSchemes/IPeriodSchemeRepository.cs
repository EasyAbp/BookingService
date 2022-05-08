using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.PeriodSchemes;

public interface IPeriodSchemeRepository : IRepository<PeriodScheme, Guid>
{
    Task<PeriodScheme> GetDefaultSchemeAsync();
}