using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.PeriodSchemes;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyManager : DomainService, IAssetOccupancyManager, IUnitOfWorkEnabled
{
    private readonly IAssetOccupancyRepository _repository;
    private readonly IAssetRepository _assetRepository;
    private readonly IPeriodSchemeRepository _periodSchemeRepository;
    private readonly IAssetPeriodSchemeRepository _assetPeriodSchemeRepository;
    private readonly DefaultPeriodSchemeStore _defaultPeriodSchemeStore;
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly IAssetScheduleRepository _assetScheduleRepository;
    private readonly IAssetScheduleSelector _assetScheduleSelector;
    private readonly BookingServiceOptions _options;

    public AssetOccupancyManager(IAssetOccupancyRepository repository,
        IAssetRepository assetRepository,
        IPeriodSchemeRepository periodSchemeRepository,
        IAssetPeriodSchemeRepository assetPeriodSchemeRepository,
        DefaultPeriodSchemeStore defaultPeriodSchemeStore,
        IAssetCategoryRepository assetCategoryRepository,
        IAssetScheduleRepository assetScheduleRepository,
        IAssetScheduleSelector assetScheduleSelector,
        IOptions<BookingServiceOptions> options)
    {
        _repository = repository;
        _assetRepository = assetRepository;
        _periodSchemeRepository = periodSchemeRepository;
        _assetPeriodSchemeRepository = assetPeriodSchemeRepository;
        _defaultPeriodSchemeStore = defaultPeriodSchemeStore;
        _assetCategoryRepository = assetCategoryRepository;
        _assetScheduleRepository = assetScheduleRepository;
        _assetScheduleSelector = assetScheduleSelector;
        _options = options.Value;
    }

    public virtual async Task<List<AssetBookableDate>> SearchAssetBookableDatesAsync(Guid assetId,
        DateTime bookingDate,
        DateTime startingDate,
        int days)
    {
        var result = new List<AssetBookableDate>();

        var defaultPeriodScheme = await _defaultPeriodSchemeStore.GetAsync();

        var endDateTime = startingDate.AddDays(days);
        var schemes = await _assetPeriodSchemeRepository.GetListByAssetIdAsync(
            assetId, startingDate, endDateTime);

        var assetSchedules = await _assetScheduleRepository.GetAssetScheduleListAfterDateAsync(assetId, startingDate);

        // remove all off range schedules
        assetSchedules.RemoveAll(x => x.GetStartingDateTime() > endDateTime);

        var asset = await _assetRepository.GetAsync(assetId);
        var category = await _assetCategoryRepository.GetAsync(asset.AssetCategoryId);

        for (var i = 0; i < days; i++)
        {
            var date = startingDate.AddDays(i);
            var assetBookableDate = new AssetBookableDate
            {
                Date = date
            };

            var effectivePeriodScheme = await CalculateEffectivePeriodSchemeIdAsync(
                schemes, date, asset, category, defaultPeriodScheme);

            foreach (var period in effectivePeriodScheme.Periods)
            {
                // get schedule of current period 
                var assetSchedule = await _assetScheduleSelector.SelectAsync(assetSchedules,
                    date, period.StartingTime, period.Duration);

                var effectiveSchedulePolicy = await CalculateEffectiveSchedulePolicyAsync(
                    assetSchedule, asset, category);

                if (effectiveSchedulePolicy == AssetSchedulePolicy.Reject)
                {
                    // TODO reject
                }
                else
                {
                    // Check whether can be booked
                    var effectiveTimeInAdvance = await CalculateEffectiveTimeInAdvanceAsync(
                        assetSchedule, asset, category);

                    if (effectiveTimeInAdvance.CanBook(date + period.StartingTime - bookingDate))
                    {
                        // Check already occupied
                        var conflictedOccupancy =
                            await GetConflictedOccupancyAsync(assetId, date, period.StartingTime, period.Duration);
                        if (conflictedOccupancy is null)
                        {
                            assetBookableDate.BookablePeriods.Add(new AssetBookablePeriod
                            {
                                StartingTime = period.StartingTime,
                                Duration = period.Duration
                            });
                        }
                    }
                }
            }

            result.Add(assetBookableDate);
        }

        return result;
    }

    public virtual async Task<List<CategoryBookableDate>> SearchCategoryBookableDatesAsync(Guid categoryId,
        DateTime bookingDate,
        DateTime startingDate,
        int days)
    {
        throw new NotImplementedException();
    }


    [UnitOfWork]
    public virtual async Task<AssetOccupancy> CreateAsync(Guid assetId, DateTime date, TimeSpan startingTime,
        TimeSpan duration,
        Guid? occupierUserId)
    {
        // TODO Distributed lock interceptor outer UOW

        throw new NotImplementedException();
    }

    [UnitOfWork]
    public virtual Task UpdateAsync(AssetOccupancy entity, Guid assetId, DateTime date, TimeSpan startingTime,
        TimeSpan duration,
        Guid? occupierUserId)
    {
        // TODO Distributed lock interceptor outer UOW

        throw new NotImplementedException();
    }

    [ItemCanBeNull]
    protected virtual async Task<AssetOccupancy> GetConflictedOccupancyAsync(Guid assetId, DateTime date,
        TimeSpan startingTime,
        TimeSpan duration)
    {
        var endDateTime = date + startingTime + duration;
        var assetOccupancies = await _repository.GetListAsync(x => x.AssetId == assetId
                                                                   && x.Date >= date);

        assetOccupancies.RemoveAll(x => x.GetStartingDateTime() > endDateTime);

        return assetOccupancies.FirstOrDefault(x => x.IsConflicted(date, startingTime, duration));
    }

    protected virtual async Task<PeriodScheme> CalculateEffectivePeriodSchemeIdAsync(List<AssetPeriodScheme> schemes,
        DateTime date,
        Asset asset,
        AssetCategory category,
        PeriodScheme defaultPeriodScheme)
    {
        // Fallback chain: AssetPeriodScheme -> Asset -> Category -> DefaultPeriodScheme
        var scheme = schemes.FirstOrDefault(x => x.Date == date);
        if (scheme is not null)
        {
            return await _periodSchemeRepository.GetAsync(scheme.PeriodSchemeId);
        }

        if (asset.PeriodSchemeId.HasValue)
        {
            return await _periodSchemeRepository.GetAsync(asset.PeriodSchemeId.Value);
        }

        if (category.PeriodSchemeId.HasValue)
        {
            return await _periodSchemeRepository.GetAsync(category.PeriodSchemeId.Value);
        }

        return defaultPeriodScheme;
    }

    protected virtual Task<AssetSchedulePolicy> CalculateEffectiveSchedulePolicyAsync(
        [CanBeNull] AssetSchedule assetSchedule, Asset asset, AssetCategory category)
    {
        // Fallback chain: AssetSchedule -> Asset -> Category -> AssetDefinition
        if (assetSchedule is not null)
        {
            return Task.FromResult(assetSchedule.SchedulePolicy);
        }

        if (asset.DefaultSchedulePolicy.HasValue)
        {
            return Task.FromResult(asset.DefaultSchedulePolicy.Value);
        }

        if (category.DefaultSchedulePolicy.HasValue)
        {
            return Task.FromResult(category.DefaultSchedulePolicy.Value);
        }

        var assetDefinition = _options.AssetDefinitions.FirstOrDefault(x => x.Name == asset.AssetDefinitionName);
        if (assetDefinition is not null)
        {
            return Task.FromResult(assetDefinition.DefaultSchedulePolicy);
        }
        else
        {
            throw new AssetDefinitionNotExistsException(asset.AssetDefinitionName);
        }
    }

    protected virtual Task<TimeInAdvance> CalculateEffectiveTimeInAdvanceAsync(
        AssetSchedule assetSchedule, Asset asset, AssetCategory category)
    {
        // Fallback chain: AssetSchedule -> Asset -> Category -> AssetDefinition
        if (assetSchedule is not null && assetSchedule.TimeInAdvance is not null)
        {
            return Task.FromResult(assetSchedule.TimeInAdvance);
        }

        if (asset.TimeInAdvance is not null)
        {
            return Task.FromResult(asset.TimeInAdvance);
        }

        if (category.TimeInAdvance is not null)
        {
            return Task.FromResult(category.TimeInAdvance);
        }

        var assetDefinition = _options.AssetDefinitions.FirstOrDefault(x => x.Name == asset.AssetDefinitionName);
        if (assetDefinition is not null)
        {
            return Task.FromResult(assetDefinition.TimeInAdvance);
        }
        else
        {
            throw new AssetDefinitionNotExistsException(asset.AssetDefinitionName);
        }
    }
}