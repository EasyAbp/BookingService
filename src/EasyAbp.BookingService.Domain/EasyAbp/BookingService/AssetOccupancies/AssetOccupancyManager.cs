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

public class AssetOccupancyManager : DomainService, IUnitOfWorkEnabled
{
    private readonly IAssetOccupancyRepository _repository;
    private readonly IAssetRepository _assetRepository;
    private readonly IPeriodSchemeRepository _periodSchemeRepository;
    private readonly IAssetPeriodSchemeRepository _assetPeriodSchemeRepository;
    private readonly DefaultPeriodSchemeStore _defaultPeriodSchemeStore;
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly IAssetScheduleRepository _assetScheduleRepository;
    private readonly BookingServiceOptions _options;

    public AssetOccupancyManager(IAssetOccupancyRepository repository,
        IAssetRepository assetRepository,
        IPeriodSchemeRepository periodSchemeRepository,
        IAssetPeriodSchemeRepository assetPeriodSchemeRepository,
        DefaultPeriodSchemeStore defaultPeriodSchemeStore,
        IAssetCategoryRepository assetCategoryRepository,
        IAssetScheduleRepository assetScheduleRepository,
        IOptions<BookingServiceOptions> options)
    {
        _repository = repository;
        _assetRepository = assetRepository;
        _periodSchemeRepository = periodSchemeRepository;
        _assetPeriodSchemeRepository = assetPeriodSchemeRepository;
        _defaultPeriodSchemeStore = defaultPeriodSchemeStore;
        _assetCategoryRepository = assetCategoryRepository;
        _assetScheduleRepository = assetScheduleRepository;
        _options = options.Value;
    }

    [UnitOfWork]
    public virtual async Task<List<BookablePeriod>> SearchAssetBookablePeriodsAsync(Guid assetId,
        DateTime bookingDateTime,
        DateTime searchDate)
    {
        var asset = await _assetRepository.GetAsync(assetId);
        if (asset.Disabled)
        {
            return new List<BookablePeriod>();
        }

        var category = await _assetCategoryRepository.GetAsync(asset.AssetCategoryId);
        if (category.Disabled)
        {
            return new List<BookablePeriod>();
        }

        var assetPeriodScheme = await _assetPeriodSchemeRepository.FindAsync(x =>
            x.AssetId == assetId && x.Date == searchDate);

        var effectivePeriodScheme = await CalculateEffectivePeriodSchemeIdAsync(
            assetPeriodScheme, asset, category);

        if (effectivePeriodScheme.Periods.IsNullOrEmpty())
        {
            return new List<BookablePeriod>();
        }

        var endingTime = effectivePeriodScheme.GetLatestEndingTime(effectivePeriodScheme);
        var endingDateTime = searchDate.Add(endingTime);

        var appliedAssetSchedules =
            await _assetScheduleRepository.GetAssetSchedulesAsync(assetId, searchDate, endingDateTime);

        var effectiveSchedules =
            await CalculateEffectiveSchedulesAsync(searchDate, endingDateTime, appliedAssetSchedules, asset, category);

        var bookablePeriods =
            await CalculateBookablePeriodsAsync(effectivePeriodScheme, effectiveSchedules, searchDate,
                bookingDateTime);

        return await FilterAssetOccupiedPeriodsAsync(asset, searchDate, bookablePeriods);
    }

    [UnitOfWork]
    public virtual async Task<List<Period>> SearchCategoryBookablePeriodsAsync(Guid categoryId,
        DateTime bookingDateTime, DateTime searchDate)
    {
        throw new NotImplementedException();
    }

    [UnitOfWork]
    public virtual async Task<AssetOccupancy> CreateAsync(Guid? assetId, Guid? categoryId, DateTime date,
        TimeSpan startingTime,
        TimeSpan duration,
        Guid? occupierUserId)
    {
        // TODO create new uow

        throw new NotImplementedException();
    }

    protected virtual async Task<PeriodScheme> CalculateEffectivePeriodSchemeIdAsync(
        [CanBeNull] AssetPeriodScheme assetPeriodScheme,
        Asset asset,
        AssetCategory category)
    {
        // Fallback chain: AssetPeriodScheme -> Asset -> Category -> DefaultPeriodScheme
        if (assetPeriodScheme is not null)
        {
            return await _periodSchemeRepository.GetAsync(assetPeriodScheme.PeriodSchemeId);
        }

        if (asset.PeriodSchemeId.HasValue)
        {
            return await _periodSchemeRepository.GetAsync(asset.PeriodSchemeId.Value);
        }

        if (category.PeriodSchemeId.HasValue)
        {
            return await _periodSchemeRepository.GetAsync(category.PeriodSchemeId.Value);
        }

        return await _defaultPeriodSchemeStore.GetAsync();
    }

    protected virtual Task<TimeInAdvance> CalculateEffectiveTimeInAdvanceAsync(Asset asset, AssetCategory category)
    {
        // Fallback chain: Asset -> Category -> AssetDefinition
        if (asset.TimeInAdvance is not null)
        {
            return Task.FromResult(asset.TimeInAdvance);
        }

        if (category.TimeInAdvance is not null)
        {
            return Task.FromResult(category.TimeInAdvance);
        }

        var assetDefinition =
            _options.AssetDefinitionConfigurations.FirstOrDefault(x => x.Name == asset.AssetDefinitionName);
        if (assetDefinition is not null)
        {
            return Task.FromResult(assetDefinition.TimeInAdvance);
        }
        else
        {
            throw new AssetDefinitionNotExistsException(asset.AssetDefinitionName);
        }
    }

    protected virtual async Task<List<BookablePeriod>> FilterAssetOccupiedPeriodsAsync(
        Asset asset,
        DateTime searchDate,
        List<BookablePeriod> bookablePeriods)
    {
        var filtered = new List<BookablePeriod>();
        var assetOccupancies = await _repository.GetListAsync(x => x.AssetId == asset.Id && x.Date == searchDate);
        foreach (var bookablePeriod in bookablePeriods.OrderBy(x => x.StartingTime))
        {
            var list = assetOccupancies
                .Where(x => bookablePeriod.IsIntersected(x.StartingTime, x.GetEndingTime()))
                .ToList();
            if (list.Count > 0)
            {
                if (!bookablePeriod.Divisible)
                {
                    continue;
                }

                // Divide period
                var startingPoint = bookablePeriod.StartingTime;
                foreach (var assetOccupancy in list.OrderBy(x => x.StartingTime))
                {
                    if (assetOccupancy.StartingTime > startingPoint)
                    {
                        filtered.Add(new BookablePeriod
                        {
                            StartingTime = startingPoint,
                            Divisible = bookablePeriod.Divisible,
                            EndingTime = assetOccupancy.StartingTime
                        });
                    }

                    startingPoint = assetOccupancy.GetEndingTime();
                }
            }
            else
            {
                filtered.Add(bookablePeriod);
            }
        }

        return filtered;
    }

    protected virtual Task<List<BookablePeriod>> CalculateBookablePeriodsAsync(PeriodScheme periodScheme,
        List<Schedule> effectiveSchedules,
        DateTime searchDate,
        DateTime bookingDateTime)
    {
        var result = new List<BookablePeriod>();
        foreach (var effectiveSchedule in effectiveSchedules
                     .Where(x => x.PeriodUsable == PeriodUsable.Accept)
                     .OrderBy(x => x.StartingDateTime))
        {
            // get current schedule's periods
            var periods = periodScheme.Periods
                .Where(x => effectiveSchedule.IsTimeRangeIntersected(searchDate + x.StartingTime,
                    searchDate + x.GetEndingTime()))
                .ToList();
            if (periods.IsNullOrEmpty())
            {
                continue;
            }

            // get bookable period
            var bookablePeriods = effectiveSchedule.CalculateBookablePeriods(searchDate, periods);

            // remove periods which cannot be occupied
            bookablePeriods.RemoveAll(x =>
                !effectiveSchedule.TimeInAdvance.CanOccupy(searchDate + x.StartingTime, bookingDateTime));

            result.AddRange(bookablePeriods);
        }

        return Task.FromResult(result);
    }

    protected virtual async Task<List<Schedule>> CalculateEffectiveSchedulesAsync(
        DateTime startingDateTime,
        DateTime endingDateTime,
        List<AssetSchedule> appliedAssetSchedules,
        Asset asset,
        AssetCategory category)
    {
        var effectiveSchedules = new List<Schedule>();

        foreach (var assetSchedule in appliedAssetSchedules
                     .Where(x => x.PeriodUsable == PeriodUsable.Accept))
        {
            effectiveSchedules.Add(new Schedule
            {
                PeriodUsable = PeriodUsable.Accept,
                StartingDateTime = assetSchedule.StartingDateTime <= startingDateTime
                    ? startingDateTime
                    : assetSchedule.StartingDateTime,
                EndingDateTime = assetSchedule.EndingDateTime >= endingDateTime
                    ? endingDateTime
                    : assetSchedule.EndingDateTime,
                TimeInAdvance = assetSchedule.TimeInAdvance
            });
        }

        foreach (var assetSchedule in appliedAssetSchedules
                     .Where(x => x.PeriodUsable == PeriodUsable.Reject))
        {
            // override accept policy
            var rejectSchedule = new Schedule
            {
                PeriodUsable = PeriodUsable.Reject,
                StartingDateTime = assetSchedule.StartingDateTime <= startingDateTime
                    ? startingDateTime
                    : assetSchedule.StartingDateTime,
                EndingDateTime = assetSchedule.EndingDateTime >= endingDateTime
                    ? endingDateTime
                    : assetSchedule.EndingDateTime,
                TimeInAdvance = assetSchedule.TimeInAdvance
            };

            var conflictedSchedule = effectiveSchedules.FirstOrDefault(x =>
                x.IsTimeRangeIntersected(rejectSchedule.StartingDateTime, rejectSchedule.EndingDateTime));
            if (conflictedSchedule is not null)
            {
                effectiveSchedules.Remove(conflictedSchedule);
                // split conflicted schedule
                effectiveSchedules.AddRange(
                    conflictedSchedule.RemoveIntersectingTimeRange(
                        rejectSchedule.StartingDateTime,
                        rejectSchedule.EndingDateTime));
            }

            effectiveSchedules.Add(rejectSchedule);
        }

        effectiveSchedules.RemoveAll(x => x.IsEmptyTimeRange());

        var effectiveTimeInAdvance = await CalculateEffectiveTimeInAdvanceAsync(asset, category);

        foreach (var effectiveSchedule in effectiveSchedules)
        {
            effectiveSchedule.TimeInAdvance ??= effectiveTimeInAdvance;
        }

        // use default policy and it's fallback to fill the time gap 
        var gaps = FindTimeGaps(startingDateTime, endingDateTime, effectiveSchedules);
        if (gaps.Count > 0)
        {
            var effectivePeriodUsable = await CalculateEffectivePeriodUsableAsync(asset, category);
            foreach (var schedule in gaps)
            {
                schedule.PeriodUsable = effectivePeriodUsable;
                schedule.TimeInAdvance = effectiveTimeInAdvance;
                effectiveSchedules.Add(schedule);
            }
        }

        return effectiveSchedules;
    }

    protected virtual Task<PeriodUsable> CalculateEffectivePeriodUsableAsync(Asset asset,
        AssetCategory category)
    {
        // Fallback chain: Asset -> Category -> AssetDefinition
        if (asset.DefaultPeriodUsable.HasValue)
        {
            return Task.FromResult(asset.DefaultPeriodUsable.Value);
        }

        if (category.DefaultPeriodUsable.HasValue)
        {
            return Task.FromResult(category.DefaultPeriodUsable.Value);
        }

        var assetDefinition =
            _options.AssetDefinitionConfigurations.FirstOrDefault(x => x.Name == asset.AssetDefinitionName);
        if (assetDefinition is not null)
        {
            return Task.FromResult(assetDefinition.DefaultPeriodUsable);
        }
        else
        {
            throw new AssetDefinitionNotExistsException(asset.AssetDefinitionName);
        }
    }

    protected virtual List<Schedule> FindTimeGaps(DateTime startingDateTime, DateTime endingDateTime,
        List<Schedule> effectiveSchedules)
    {
        var gaps = new List<Schedule>();
        if (effectiveSchedules.Count == 0)
        {
            gaps.Add(new Schedule
            {
                StartingDateTime = startingDateTime,
                EndingDateTime = endingDateTime
            });
        }
        else
        {
            var start = startingDateTime;
            foreach (var schedule in effectiveSchedules.OrderBy(x => x.StartingDateTime))
            {
                if (schedule.StartingDateTime != start)
                {
                    gaps.Add(new Schedule
                    {
                        StartingDateTime = start,
                        EndingDateTime = schedule.StartingDateTime
                    });
                }

                start = schedule.EndingDateTime;
            }

            if (start != endingDateTime)
            {
                gaps.Add(new Schedule
                {
                    StartingDateTime = start,
                    EndingDateTime = endingDateTime
                });
            }
        }

        return gaps;
    }
}