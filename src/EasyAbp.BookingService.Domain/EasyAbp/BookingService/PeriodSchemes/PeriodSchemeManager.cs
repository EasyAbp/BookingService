using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeManager : DomainService, IUnitOfWorkEnabled
{
    private readonly IPeriodSchemeRepository _repository;
    private readonly DefaultPeriodSchemeStore _defaultPeriodSchemeStore;

    public PeriodSchemeManager(IPeriodSchemeRepository repository,
        DefaultPeriodSchemeStore defaultPeriodSchemeStore)
    {
        _repository = repository;
        _defaultPeriodSchemeStore = defaultPeriodSchemeStore;
    }

    [UnitOfWork]
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        if (entity.IsDefault)
        {
            throw new CannotDeleteDefaultPeriodSchemeException(entity.Name);
        }

        await _repository.DeleteAsync(entity);
    }

    [UnitOfWork(isTransactional: true)]
    public virtual async Task<PeriodScheme> SetAsDefaultAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        if (!entity.IsDefault)
        {
            await _defaultPeriodSchemeStore.ClearAsync();

            var defaultPeriodScheme = await _repository.FindAsync(x => x.IsDefault);
            if (defaultPeriodScheme is not null)
            {
                defaultPeriodScheme.UpdateIsDefault(false);
                await _repository.UpdateAsync(defaultPeriodScheme);
            }

            entity.UpdateIsDefault(true);
            entity = await _repository.UpdateAsync(entity);
        }

        return entity;
    }

    public virtual async Task<PeriodScheme> CreateAsync(string name, List<Period> periods)
    {
        var defaultPeriodScheme = await _repository.FindAsync(x => x.IsDefault);

        await _defaultPeriodSchemeStore.ClearAsync();

        return new PeriodScheme(GuidGenerator.Create(),
            CurrentTenant.Id,
            name,
            defaultPeriodScheme is null, // If there is no default period scheme, set this entity as default
            periods);
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