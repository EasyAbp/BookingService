using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeManager : DomainService
{
    protected IPeriodSchemeRepository Repository { get; }

    public PeriodSchemeManager(IPeriodSchemeRepository repository)
    {
        Repository = repository;
    }

    public virtual Task<PeriodScheme> CreateAsync(string name, List<Period> periods)
    {
        return Task.FromResult(new PeriodScheme(GuidGenerator.Create(),
            CurrentTenant.Id,
            name,
            false,
            periods));
    }

    public virtual Task<Period> CreatePeriodAsync(TimeSpan startingTime, TimeSpan duration)
    {
        return Task.FromResult(new Period(GuidGenerator.Create(), startingTime, duration));
    }

    public virtual Task<Period> UpdatePeriodAsync(PeriodScheme periodScheme, Guid periodId, TimeSpan startingTime,
        TimeSpan duration)
    {
        var period = periodScheme.Periods.Single(x => x.Id == periodId);

        period.Update(startingTime, duration);

        return Task.FromResult(period);
    }

    public virtual Task UnsetDefaultAsync(PeriodScheme entity)
    {
        if (!entity.IsDefault)
        {
            return Task.CompletedTask;
        }

        entity.UpdateIsDefault(false);
        return Task.CompletedTask;
    }

    public virtual async Task SetAsDefaultAsync(PeriodScheme entity)
    {
        if (entity.IsDefault)
        {
            return;
        }

        var defaultPeriodScheme = await Repository.FindDefaultSchemeAsync();
        if (defaultPeriodScheme is not null)
        {
            throw new DefaultPeriodSchemeAlreadyExistsException();
        }

        entity.UpdateIsDefault(true);
    }
}