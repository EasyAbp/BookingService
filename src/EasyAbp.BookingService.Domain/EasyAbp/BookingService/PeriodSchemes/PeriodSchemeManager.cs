using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeManager : DomainService
{
    public virtual Task<PeriodScheme> CreateAsync(string name, List<Period> periods)
    {
        return Task.FromResult(new PeriodScheme(GuidGenerator.Create(),
            CurrentTenant.Id,
            name,
            false,
            periods));
    }

    public virtual Task UpdateAsync(PeriodScheme entity, string name, List<Period> periods)
    {
        var newPeriods = new List<Period>();
        if (!periods.IsNullOrEmpty())
        {
            for (var i = 0; i < periods.Count; i++)
            {
                var period = entity.Periods.ElementAtOrDefault(i);
                if (period is null)
                {
                    period = new Period(GuidGenerator.Create(),
                        periods[i].StartingTime,
                        periods[i].Duration,
                        periods[i].Divisible);
                }
                else
                {
                    period.Update(periods[i].StartingTime,
                        periods[i].Duration,
                        periods[i].Divisible);
                }

                newPeriods.Add(period);
            }
        }

        entity.Update(name, newPeriods);
        return Task.CompletedTask;
    }
}