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

public class AssetOccupancyManager : DomainService
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

        var effectivePeriodScheme = await CalculateEffectivePeriodSchemeAsync(
            assetPeriodScheme, asset, category);

        if (effectivePeriodScheme.Periods.IsNullOrEmpty())
        {
            return new List<BookablePeriod>();
        }

        var endingTime = effectivePeriodScheme.Periods
            .Select(x => x.GetEndingTime())
            .OrderByDescending(x => x)
            .First();

        var endingDateTime = searchDate.Add(endingTime);

        var appliedAssetSchedules =
            await _assetScheduleRepository.GetAssetScheduleListInScopeAsync(assetId, searchDate, endingDateTime);

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

    protected virtual async Task<PeriodScheme> CalculateEffectivePeriodSchemeAsync(
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
        // Fallback chain when no schedule: Asset -> Category -> AssetDefinition
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
            var conflictOccupancies = assetOccupancies
                .Where(x => bookablePeriod.IsIntersected(x.StartingTime, x.GetEndingTime()))
                .ToList();
            if (conflictOccupancies.Count > 0)
            {
                if (!bookablePeriod.Divisible)
                {
                    continue;
                }

                // Divide period
                var startingPoint = bookablePeriod.StartingTime;
                foreach (var assetOccupancy in conflictOccupancies.OrderBy(x => x.StartingTime))
                {
                    if (assetOccupancy.StartingTime > startingPoint)
                    {
                        filtered.Add(new BookablePeriod
                        {
                            StartingTime = startingPoint,
                            EndingTime = assetOccupancy.StartingTime,
                            Divisible = bookablePeriod.Divisible,
                            PeriodId = bookablePeriod.PeriodId,
                            PeriodSchemeId = bookablePeriod.PeriodSchemeId
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
        List<EffectiveScheduleModel> effectiveSchedules,
        DateTime searchDate,
        DateTime bookingDateTime)
    {
        var result = new List<BookablePeriod>();
        if (periodScheme.Periods.IsNullOrEmpty())
        {
            return Task.FromResult(result);
        }

        foreach (var effectiveSchedule in effectiveSchedules
                     .Where(x => x.PeriodUsable == PeriodUsable.Accept)
                     .OrderBy(x => x.StartingDateTime))
        {
            // get bookable period
            var bookablePeriods = effectiveSchedule.CalculateBookablePeriods(searchDate, periodScheme);

            // remove periods which cannot be occupied
            bookablePeriods.RemoveAll(x =>
                !effectiveSchedule.TimeInAdvance.CanOccupy(searchDate + x.StartingTime, bookingDateTime));

            result.AddRange(bookablePeriods);
        }

        return Task.FromResult(result);
    }

    protected virtual async Task<List<EffectiveScheduleModel>> CalculateEffectiveSchedulesAsync(
        DateTime startingDateTime,
        DateTime endingDateTime,
        List<AssetSchedule> appliedAssetSchedules,
        Asset asset,
        AssetCategory category)
    {
        var effectiveSchedules = new List<EffectiveScheduleModel>();

        foreach (var assetSchedule in appliedAssetSchedules
                     .Where(x => x.PeriodUsable == PeriodUsable.Accept))
        {
            effectiveSchedules.Add(new EffectiveScheduleModel(
                assetSchedule.StartingDateTime <= startingDateTime
                    ? startingDateTime
                    : assetSchedule.StartingDateTime,
                assetSchedule.EndingDateTime >= endingDateTime
                    ? endingDateTime
                    : assetSchedule.EndingDateTime,
                PeriodUsable.Accept,
                assetSchedule.TimeInAdvance
            ));
        }

        foreach (var assetSchedule in appliedAssetSchedules
                     .Where(x => x.PeriodUsable == PeriodUsable.Reject))
        {
            // override accept policy
            var rejectSchedule = new EffectiveScheduleModel(
                assetSchedule.StartingDateTime <= startingDateTime
                    ? startingDateTime
                    : assetSchedule.StartingDateTime,
                assetSchedule.EndingDateTime >= endingDateTime
                    ? endingDateTime
                    : assetSchedule.EndingDateTime,
                PeriodUsable.Reject,
                assetSchedule.TimeInAdvance);

            var conflictedSchedule = effectiveSchedules.FirstOrDefault(x =>
                x.IsTimeRangeIntersected(rejectSchedule.StartingDateTime, rejectSchedule.EndingDateTime));
            if (conflictedSchedule is not null)
            {
                effectiveSchedules.Remove(conflictedSchedule);
                // split conflicted schedule
                effectiveSchedules.AddRange(
                    conflictedSchedule.ExcludeIntersectedAndCreateNewSchedules(
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

        // Get no-schedule time ranges of the day and convert them to EffectiveScheduleModel and fall back to the default PeriodUsable.
        var scheduleModels = await GenerateFallbackEffectiveScheduleModelsAsync(
            startingDateTime, endingDateTime, asset, category, effectiveSchedules, effectiveTimeInAdvance);

        effectiveSchedules.AddRange(scheduleModels);

        return effectiveSchedules;
    }

    protected virtual async Task<List<EffectiveScheduleModel>> GenerateFallbackEffectiveScheduleModelsAsync(
        DateTime startingDateTime, DateTime endingDateTime,
        Asset asset, AssetCategory category, List<EffectiveScheduleModel> effectiveSchedules,
        TimeInAdvance effectiveTimeInAdvance)
    {
        var scheduleModels = new List<EffectiveScheduleModel>();
        var effectivePeriodUsable = await CalculateEffectivePeriodUsableAsync(asset, category);
        if (effectiveSchedules.Count == 0)
        {
            scheduleModels.Add(new EffectiveScheduleModel(
                startingDateTime,
                endingDateTime,
                effectivePeriodUsable,
                effectiveTimeInAdvance));
        }
        else
        {
            var start = startingDateTime;
            foreach (var schedule in effectiveSchedules.OrderBy(x => x.StartingDateTime))
            {
                if (schedule.StartingDateTime != start)
                {
                    scheduleModels.Add(new EffectiveScheduleModel(
                        start,
                        schedule.StartingDateTime,
                        effectivePeriodUsable,
                        effectiveTimeInAdvance));
                }

                start = schedule.EndingDateTime;
            }

            if (start != endingDateTime)
            {
                scheduleModels.Add(new EffectiveScheduleModel(
                    start,
                    endingDateTime,
                    effectivePeriodUsable,
                    effectiveTimeInAdvance));
            }
        }

        return scheduleModels;
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
}