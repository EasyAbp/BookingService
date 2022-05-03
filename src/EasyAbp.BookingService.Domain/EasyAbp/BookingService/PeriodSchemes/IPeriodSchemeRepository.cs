using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.PeriodSchemes;

public interface IPeriodSchemeRepository : IRepository<PeriodScheme, Guid>
{
}