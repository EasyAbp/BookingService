using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

    public virtual Task UpdateAsync(PeriodScheme entity, string name, [NotNull] List<Period> inputPeriods)
    {
        entity.Periods.RemoveAll(x => !inputPeriods.Select(y => y.Id).Contains(x.Id));

        foreach (var inputPeriod in inputPeriods)
        {
            var period = entity.Periods.Find(x => x.Id == inputPeriod.Id);

            if (period is null)
            {
                entity.Periods.Add(new Period(GuidGenerator.Create(), inputPeriod.StartingTime, inputPeriod.Duration));
            }
            else
            {
                period.Update(inputPeriod.StartingTime, inputPeriod.Duration);
            }
        }

        entity.Update(name);
        return Task.CompletedTask;
    }
}