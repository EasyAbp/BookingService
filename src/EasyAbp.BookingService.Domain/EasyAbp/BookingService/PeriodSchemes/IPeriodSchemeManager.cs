using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.BookingService.PeriodSchemes;

public interface IPeriodSchemeManager
{
    Task DeleteAsync(Guid id);
    Task<PeriodScheme> SetAsDefaultAsync(Guid id);
    Task<PeriodScheme> CreateAsync(string name, List<Period> periods);
    Task UpdateAsync(PeriodScheme entity, string name, List<Period> periods);
}