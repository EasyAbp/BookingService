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
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyManager : DomainService
{
    private readonly IAssetOccupancyRepository _repository;
    private readonly IPeriodSchemeRepository _periodSchemeRepository;
    private readonly IAssetPeriodSchemeRepository _assetPeriodSchemeRepository;
    private readonly DefaultPeriodSchemeProvider _defaultPeriodSchemeProvider;
    private readonly IAssetScheduleRepository _assetScheduleRepository;
    private readonly BookingServiceOptions _options;

    public AssetOccupancyManager(IAssetOccupancyRepository repository,
        IPeriodSchemeRepository periodSchemeRepository,
        IAssetPeriodSchemeRepository assetPeriodSchemeRepository,
        DefaultPeriodSchemeProvider defaultPeriodSchemeProvider,
        IAssetScheduleRepository assetScheduleRepository,
        IOptions<BookingServiceOptions> options)
    {
        _repository = repository;
        _periodSchemeRepository = periodSchemeRepository;
        _assetPeriodSchemeRepository = assetPeriodSchemeRepository;
        _defaultPeriodSchemeProvider = defaultPeriodSchemeProvider;
        _assetScheduleRepository = assetScheduleRepository;
        _options = options.Value;
    }

    [UnitOfWork]
    public virtual async Task<List<PeriodOccupancyModel>> SearchAssetBookablePeriodsAsync(Asset asset,
        AssetCategory category,
        DateTime currentDateTime,
        DateTime targetDate)
    {
        var periodScheme = await GetEffectivePeriodSchemeAsync(targetDate, asset, category);
        var defaultAvailable = await GetEffectivePeriodUsableAsync(asset, category) is PeriodUsable.Accept;
        var timeInAdvance = await GetEffectiveTimeInAdvanceAsync(asset, category);

        var models = periodScheme.Periods.Select(x => new PeriodOccupancyModel(targetDate, x.StartingTime,
            x.GetEndingTime(), periodScheme.Id, x.Id, defaultAvailable)).ToList();

        var schedules = await _assetScheduleRepository.GetListAsync(targetDate, asset.Id, periodScheme.Id);
        var occupancies = await _repository.GetListAsync(targetDate, asset.Id);

        UpdatePeriodsUsableBySchedules(models, schedules);
        UpdatePeriodsUsableByOccupancies(models, occupancies);
        UpdatePeriodsUsableByTimeInAdvances(models, timeInAdvance, currentDateTime);

        return models;
    }

    [UnitOfWork]
    public virtual async Task<List<PeriodOccupancyModel>> SearchCategoryBookablePeriodsAsync(Guid categoryId,
        DateTime currentDateTime, DateTime targetDate)
    {
        throw new NotImplementedException();
    }

    [UnitOfWork]
    public virtual async Task<AssetOccupancy> CreateAsync(OccupyAssetInfoModel model, Guid? occupierUserId)
    {
        throw new NotImplementedException();
    }

    [UnitOfWork]
    public virtual async Task<AssetOccupancy> CreateByCategoryIdAsync(OccupyAssetByCategoryInfoModel model,
        Guid? occupierUserId)
    {
        throw new NotImplementedException();
    }

    [UnitOfWork(true)]
    public virtual async Task<AssetOccupancy> BulkCreateAsync(List<OccupyAssetInfoModel> models,
        List<OccupyAssetByCategoryInfoModel> byCateModels, Guid? occupierUserId)
    {
        throw new NotImplementedException();
    }

    protected virtual async Task<PeriodScheme> GetEffectivePeriodSchemeAsync(
        DateTime date,
        Asset asset,
        AssetCategory category)
    {
        var assetPeriodScheme = await _assetPeriodSchemeRepository.FindAsync(x =>
            x.AssetId == asset.Id && x.Date == date);

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

        return await _defaultPeriodSchemeProvider.GetAsync();
    }


    protected virtual Task<PeriodUsable> GetEffectivePeriodUsableAsync(Asset asset, AssetCategory category)
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

    protected virtual Task<TimeInAdvance> GetEffectiveTimeInAdvanceAsync(Asset asset, AssetCategory category)
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
        if (assetDefinition is null)
        {
            throw new AssetDefinitionNotExistsException(asset.AssetDefinitionName);
        }

        return Task.FromResult(assetDefinition.TimeInAdvance);
    }

    protected virtual void UpdatePeriodsUsableBySchedules(
        List<PeriodOccupancyModel> models,
        List<AssetSchedule> schedules)
    {
        foreach (var schedule in schedules)
        {
            foreach (var model in models.Where(x =>
                         x.PeriodSchemeId == schedule.PeriodSchemeId && x.PeriodId == schedule.PeriodId))
            {
                model.Available = schedule.PeriodUsable is PeriodUsable.Accept;
            }
        }
    }

    protected virtual void UpdatePeriodsUsableByOccupancies(
        List<PeriodOccupancyModel> models,
        List<AssetOccupancy> occupancies)
    {
        foreach (var occupancy in occupancies)
        {
            foreach (var model in models.Where(x =>
                         x.IsIntersected(occupancy.Date, occupancy.StartingTime, occupancy.GetEndingTime())))
            {
                model.Available = false;
            }
        }
    }

    protected virtual void UpdatePeriodsUsableByTimeInAdvances(
        IEnumerable<PeriodOccupancyModel> models,
        TimeInAdvance timeInAdvance,
        DateTime currentDateTime)
    {
        foreach (var model in models.Where(x => !timeInAdvance.CanOccupy(x.GetStartingDateTime(), currentDateTime)))
        {
            model.Available = false;
        }
    }
}