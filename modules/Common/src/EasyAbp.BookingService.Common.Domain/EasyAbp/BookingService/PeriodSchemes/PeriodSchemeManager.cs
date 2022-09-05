using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeManager : DomainService
{
    protected IPeriodSchemeRepository Repository =>
        LazyServiceProvider.LazyGetRequiredService<IPeriodSchemeRepository>();

    protected IAssetCategoryRepository AssetCategoryRepository =>
        LazyServiceProvider.LazyGetRequiredService<IAssetCategoryRepository>();

    protected IAssetRepository AssetRepository =>
        LazyServiceProvider.LazyGetRequiredService<IAssetRepository>();

    protected IAssetPeriodSchemeRepository AssetPeriodSchemeRepository =>
        LazyServiceProvider.LazyGetRequiredService<IAssetPeriodSchemeRepository>();

    protected IAssetScheduleManager AssetScheduleManager =>
        LazyServiceProvider.LazyGetRequiredService<IAssetScheduleManager>();

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

    public virtual async Task<Period> UpdatePeriodAsync(PeriodScheme periodScheme, Guid periodId, TimeSpan startingTime,
        TimeSpan duration)
    {
        var period = periodScheme.Periods.Single(x => x.Id == periodId);
        if (await IsPeriodInUseAsync(period))
        {
            throw new CannotUpdatePeriodInUseException(period.Id);
        }

        period.Update(startingTime, duration);

        return period;
    }

    public virtual async Task<Period> DeletePeriodAsync(PeriodScheme periodScheme, Guid periodId)
    {
        var period = periodScheme.Periods.Single(x => x.Id == periodId);
        if (await IsPeriodInUseAsync(period))
        {
            throw new CannotDeletePeriodInUseException(period.Id);
        }

        periodScheme.Periods.Remove(period);
        return period;
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

    public virtual async Task<bool> IsPeriodSchemeInUseAsync(PeriodScheme periodScheme)
    {
        var assetCategory = await AssetCategoryRepository.FirstOrDefaultAsync(x => x.PeriodSchemeId == periodScheme.Id);
        if (assetCategory is not null)
        {
            return true;
        }

        var asset = await AssetRepository.FirstOrDefaultAsync(x => x.PeriodSchemeId == periodScheme.Id);
        if (asset is not null)
        {
            return true;
        }

        if (await AssetScheduleManager.AnyByPeriodSchemeIdAsync(periodScheme.Id))
        {
            return true;
        }

        var assetPeriodScheme =
            await AssetPeriodSchemeRepository.FirstOrDefaultAsync(x => x.PeriodSchemeId == periodScheme.Id);
        return assetPeriodScheme is not null;
    }

    protected virtual Task<bool> IsPeriodInUseAsync(Period period)
    {
        return AssetScheduleManager.AnyByPeriodIdAsync(period.Id);
    }
}