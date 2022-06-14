using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.PeriodSchemes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public abstract class AssetOccupancyProviderBase : IAssetOccupancyProvider
{
    private readonly ILogger<AssetOccupancyProviderBase> _logger;

    protected IClock Clock { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IAssetRepository AssetRepository { get; }
    protected IAssetCategoryRepository AssetCategoryRepository { get; }
    protected IAssetOccupancyRepository AssetOccupancyRepository { get; }
    protected IPeriodSchemeRepository PeriodSchemeRepository { get; }
    protected IAssetPeriodSchemeRepository AssetPeriodSchemeRepository { get; }
    protected IDefaultPeriodSchemeProvider DefaultPeriodSchemeProvider { get; }
    protected IAssetScheduleRepository AssetScheduleRepository { get; }
    protected IExternalUserLookupServiceProvider ExternalUserLookupServiceProvider { get; }
    protected BookingServiceOptions Options { get; }

    public AssetOccupancyProviderBase(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<AssetOccupancyProviderBase>>();
        Clock = serviceProvider.GetRequiredService<IClock>();
        GuidGenerator = serviceProvider.GetRequiredService<IGuidGenerator>();
        CurrentTenant = serviceProvider.GetRequiredService<ICurrentTenant>();
        UnitOfWorkManager = serviceProvider.GetRequiredService<IUnitOfWorkManager>();
        AssetRepository = serviceProvider.GetRequiredService<IAssetRepository>();
        AssetCategoryRepository = serviceProvider.GetRequiredService<IAssetCategoryRepository>();
        AssetOccupancyRepository = serviceProvider.GetRequiredService<IAssetOccupancyRepository>();
        PeriodSchemeRepository = serviceProvider.GetRequiredService<IPeriodSchemeRepository>();
        AssetPeriodSchemeRepository = serviceProvider.GetRequiredService<IAssetPeriodSchemeRepository>();
        DefaultPeriodSchemeProvider = serviceProvider.GetRequiredService<IDefaultPeriodSchemeProvider>();
        AssetScheduleRepository = serviceProvider.GetRequiredService<IAssetScheduleRepository>();
        ExternalUserLookupServiceProvider = serviceProvider.GetRequiredService<IExternalUserLookupServiceProvider>();
        Options = serviceProvider.GetRequiredService<IOptions<BookingServiceOptions>>().Value;
    }

    [UnitOfWork]
    public virtual async Task<List<PeriodOccupancyModel>> GetPeriodsAsync(Asset asset, AssetCategory categoryOfAsset,
        DateTime currentDateTime, DateTime targetDate)
    {
        var periodScheme = await GetEffectivePeriodSchemeAsync(targetDate, asset, categoryOfAsset);

        var defaultAvailable = !asset.Disabled && !categoryOfAsset.Disabled &&
                               await GetEffectivePeriodUsableAsync(asset, categoryOfAsset) is PeriodUsable.Accept;

        var defaultAvailableVolume = defaultAvailable ? asset.Volume : 0;

        var timeInAdvance = await GetEffectiveTimeInAdvanceAsync(asset, categoryOfAsset);

        var models = periodScheme.Periods.Select(x => new PeriodOccupancyModel(targetDate, x.StartingTime,
            x.GetEndingTime(), periodScheme.Id, x.Id, asset.Volume, defaultAvailableVolume)).ToList();

        var schedules = await AssetScheduleRepository.GetListAsync(targetDate, asset.Id, periodScheme.Id);
        var occupancies = await ProviderGetAssetOccupanciesAsync(targetDate, asset.Id);

        UpdatePeriodsUsableBySchedules(models, schedules);
        UpdatePeriodsUsableByTimeInAdvances(models, timeInAdvance, currentDateTime);
        UpdatePeriodsUsableByOccupancies(models, occupancies);

        return models;
    }

    [UnitOfWork]
    public virtual Task<List<PeriodOccupancyModel>> GetPeriodsAsync(AssetCategory category, DateTime currentDateTime,
        DateTime targetDate)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<bool> CanOccupyAsync(OccupyAssetInfoModel model)
    {
        var asset = await AssetRepository.GetAsync(model.AssetId);
        var category = await AssetCategoryRepository.GetAsync(asset.AssetCategoryId);

        var periods = await GetPeriodsAsync(asset, category, Clock.Now, model.Date);

        return await IsVolumeSufficientAsync(model, periods);
    }

    public virtual async Task<bool> CanOccupyByCategoryAsync(OccupyAssetByCategoryInfoModel model)
    {
        var category = await AssetCategoryRepository.GetAsync(model.AssetCategoryId);

        if (category.Disabled)
        {
            return false;
        }

        return await PickAssetOrNullAsync(category, model) is not null;
    }

    public virtual async Task<bool> CanBulkOccupyAsync(List<OccupyAssetInfoModel> models,
        List<OccupyAssetByCategoryInfoModel> byCategoryModels)
    {
        var assetDayPeriods = new Dictionary<(Guid, DateTime), List<PeriodOccupancyModel>>();

        foreach (var assetIdGroup in models.GroupBy(x => x.AssetId))
        {
            var asset = await AssetRepository.GetAsync(assetIdGroup.Key);
            var categoryOfAsset = await AssetCategoryRepository.GetAsync(asset.AssetCategoryId);

            if (asset.Disabled || categoryOfAsset.Disabled)
            {
                return false;
            }

            foreach (var dateGroup in assetIdGroup.GroupBy(x => x.Date))
            {
                var periods =
                    await GetCachedAssetDayPeriodsAsync(asset, categoryOfAsset, dateGroup.Key, assetDayPeriods);

                foreach (var model in dateGroup)
                {
                    if (!await IsVolumeSufficientAsync(model, periods))
                    {
                        return false;
                    }

                    var period = periods.First(x =>
                        x.Date == model.Date && x.StartingTime == model.StartingTime &&
                        x.EndingTime == model.GetEndingTime());

                    period.AvailableVolume -= model.Volume;
                }
            }
        }

        foreach (var categoryIdGroup in byCategoryModels.GroupBy(x => x.AssetCategoryId))
        {
            var category = await AssetCategoryRepository.GetAsync(categoryIdGroup.Key);

            if (category.Disabled)
            {
                return false;
            }

            foreach (var dateGroup in categoryIdGroup.GroupBy(x => x.Date))
            {
                foreach (var model in dateGroup)
                {
                    var assets =
                        await AssetRepository.GetListAsync(x => x.AssetCategoryId == category.Id && !x.Disabled);

                    foreach (var asset in assets)
                    {
                        var periods =
                            await GetCachedAssetDayPeriodsAsync(asset, category, dateGroup.Key, assetDayPeriods);

                        if (!await IsVolumeSufficientAsync(model, periods))
                        {
                            return false;
                        }

                        var period = periods.First(x =>
                            x.Date == model.Date && x.StartingTime == model.StartingTime &&
                            x.EndingTime == model.GetEndingTime());

                        period.AvailableVolume -= model.Volume;
                    }
                }
            }
        }

        return true;
    }

    protected virtual async Task<List<PeriodOccupancyModel>> GetCachedAssetDayPeriodsAsync(Asset asset,
        AssetCategory category, DateTime date, Dictionary<(Guid, DateTime), List<PeriodOccupancyModel>> cachedPeriods)
    {
        if (!cachedPeriods.ContainsKey((asset.Id, date)))
        {
            cachedPeriods[(asset.Id, date)] =
                await GetPeriodsAsync(asset, category, Clock.Now, date);
        }

        return cachedPeriods[(asset.Id, date)];
    }

    [UnitOfWork(true)]
    public virtual async Task<(ProviderAssetOccupancyModel, AssetOccupancy)> OccupyAsync(OccupyAssetInfoModel model,
        Guid? occupierUserId)
    {
        // Todo: lock the day?
        var asset = await AssetRepository.GetAsync(model.AssetId);

        await CheckOccupancyAsync(asset, model);

        return await InternalOccupyAsync(asset, model, occupierUserId);
    }

    [UnitOfWork(true)]
    public virtual async Task<(ProviderAssetOccupancyModel, AssetOccupancy)> OccupyByCategoryAsync(
        OccupyAssetByCategoryInfoModel model, Guid? occupierUserId)
    {
        // Todo: lock the day?
        var category = await AssetCategoryRepository.GetAsync(model.AssetCategoryId);

        if (category.Disabled)
        {
            throw new BusinessException(BookingServiceErrorCodes.DisabledAssetOrCategory);
        }

        var asset = await PickAssetOrNullAsync(category, model);

        if (asset is null)
        {
            throw new BusinessException(BookingServiceErrorCodes.InsufficientAssetVolume);
        }

        return await InternalOccupyAsync(asset, model.ToOccupyAssetInfoModel(asset.Id), occupierUserId);
    }

    [UnitOfWork(true)]
    public virtual async Task<List<(ProviderAssetOccupancyModel, AssetOccupancy)>> BulkOccupyAsync(
        List<OccupyAssetInfoModel> models, List<OccupyAssetByCategoryInfoModel> byCategoryModels, Guid? occupierUserId)
    {
        // Todo: lock the days?
        if (!await CanBulkOccupyAsync(models, byCategoryModels))
        {
            throw new BusinessException(BookingServiceErrorCodes.InsufficientAssetVolume);
        }

        var assetOccupancies = new List<(ProviderAssetOccupancyModel, AssetOccupancy)>();

        try
        {
            foreach (var model in models)
            {
                assetOccupancies.Add(await OccupyAsync(model, occupierUserId));
            }

            foreach (var model in byCategoryModels)
            {
                assetOccupancies.Add(await OccupyByCategoryAsync(model, occupierUserId));
            }
        }
        catch
        {
            foreach (var (model, _) in assetOccupancies)
            {
                if (!await ProviderTryRollBackOccupancyAsync(model))
                {
                    _logger.LogWarning("Occupancy provider occupancy rollback failed! {model}", model);
                }
            }

            throw;
        }

        return assetOccupancies;
    }

    protected virtual async Task CheckOccupancyAsync(Asset asset, OccupyAssetInfoModel model)
    {
        var categoryOfAsset = await AssetCategoryRepository.GetAsync(asset.AssetCategoryId);

        if (asset.Disabled || categoryOfAsset.Disabled)
        {
            throw new BusinessException(BookingServiceErrorCodes.DisabledAssetOrCategory);
        }

        var periods = await GetPeriodsAsync(asset, categoryOfAsset, Clock.Now, model.Date);

        if (!await IsVolumeSufficientAsync(model, periods))
        {
            throw new BusinessException(BookingServiceErrorCodes.InsufficientAssetVolume);
        }
    }

    protected virtual Task<bool> IsVolumeSufficientAsync(IOccupyingBaseInfo model,
        IEnumerable<PeriodOccupancyModel> periods)
    {
        var period = periods.First(x =>
            x.Date == model.Date && x.StartingTime == model.StartingTime && x.EndingTime == model.GetEndingTime());

        return Task.FromResult(period.AvailableVolume >= model.Volume);
    }

    protected virtual async Task<Asset> PickAssetOrNullAsync(AssetCategory category,
        OccupyAssetByCategoryInfoModel model)
    {
        var assets = await AssetRepository.GetListAsync(x => x.AssetCategoryId == category.Id && !x.Disabled);

        foreach (var asset in assets)
        {
            var periods = await GetPeriodsAsync(asset, category, Clock.Now, model.Date);

            if (await IsVolumeSufficientAsync(model, periods))
            {
                return asset;
            }
        }

        return null;
    }

    protected virtual async Task<(ProviderAssetOccupancyModel, AssetOccupancy)> InternalOccupyAsync(Asset asset,
        OccupyAssetInfoModel model, Guid? occupierUserId)
    {
        var result = await ProviderOccupyAsync(model);

        try
        {
            return (result, await CreateInsertAssetOccupancyEntityAsync(asset, model, model.Volume, occupierUserId));
        }
        catch (Exception e)
        {
            _logger.LogError("Occupancy provider threw: {e}", e);

            if (!await ProviderTryRollBackOccupancyAsync(result))
            {
                _logger.LogWarning("Occupancy provider occupancy rollback failed! {result}", result);
            }

            await UnitOfWorkManager.Current.RollbackAsync();

            throw;
        }
    }

    protected abstract Task<List<ProviderAssetOccupancyModel>> ProviderGetAssetOccupanciesAsync(
        DateTime targetDate, Guid assetId);

    protected abstract Task<ProviderAssetOccupancyModel> ProviderOccupyAsync(OccupyAssetInfoModel model);

    protected abstract Task<bool> ProviderTryRollBackOccupancyAsync(ProviderAssetOccupancyModel model);

    protected virtual async Task<string> GetOccupierNameOrNullAsync(Guid? occupierUserId)
    {
        return occupierUserId.HasValue
            ? (await ExternalUserLookupServiceProvider.FindByIdAsync(occupierUserId.Value))?.UserName
            : null;
    }

    protected virtual async Task<AssetOccupancy> CreateInsertAssetOccupancyEntityAsync(Asset asset,
        IOccupyingTimeInfo timeInfo,
        int volume, Guid? occupierUserId)
    {
        var userName = await GetOccupierNameOrNullAsync(occupierUserId);

        return await AssetOccupancyRepository.InsertAsync(new AssetOccupancy(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            asset.Id,
            asset.Name,
            asset.AssetDefinitionName,
            volume,
            timeInfo.Date,
            timeInfo.StartingTime,
            timeInfo.Duration,
            occupierUserId,
            userName), true);
    }

    protected virtual async Task<PeriodScheme> GetEffectivePeriodSchemeAsync(
        DateTime date,
        Asset asset,
        AssetCategory category)
    {
        var assetPeriodScheme = await AssetPeriodSchemeRepository.FindAsync(x =>
            x.Date == date && x.AssetId == asset.Id);

        // Fallback chain: AssetPeriodScheme -> Asset -> Category -> DefaultPeriodScheme
        if (assetPeriodScheme is not null)
        {
            return await PeriodSchemeRepository.GetAsync(assetPeriodScheme.PeriodSchemeId);
        }

        if (asset.PeriodSchemeId.HasValue)
        {
            return await PeriodSchemeRepository.GetAsync(asset.PeriodSchemeId.Value);
        }

        if (category.PeriodSchemeId.HasValue)
        {
            return await PeriodSchemeRepository.GetAsync(category.PeriodSchemeId.Value);
        }

        return await DefaultPeriodSchemeProvider.GetAsync();
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
            Options.AssetDefinitionConfigurations.FirstOrDefault(x => x.Name == asset.AssetDefinitionName);
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
            Options.AssetDefinitionConfigurations.FirstOrDefault(x => x.Name == asset.AssetDefinitionName);
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
        foreach (var model in models)
        {
            foreach (var schedule in schedules.Where(x =>
                         x.PeriodSchemeId == model.PeriodSchemeId && x.PeriodId == model.PeriodId))
            {
                model.AvailableVolume = schedule.PeriodUsable is PeriodUsable.Accept ? model.TotalVolume : 0;
            }
        }
    }

    protected virtual void UpdatePeriodsUsableByOccupancies(
        IEnumerable<PeriodOccupancyModel> models,
        IEnumerable<IOccupyingBaseInfo> occupancies)
    {
        var occupancyList = occupancies.ToList();

        foreach (var model in models.Where(x => x.AvailableVolume > 0))
        {
            foreach (var occupancy in occupancyList.Where(x =>
                         model.IsIntersected(x.Date, x.StartingTime, x.GetEndingTime())))
            {
                var newAvailableVolume = model.AvailableVolume - occupancy.Volume;

                model.AvailableVolume = newAvailableVolume >= 0 ? newAvailableVolume : 0;

                if (newAvailableVolume == 0)
                {
                    break;
                }
            }
        }
    }

    protected virtual void UpdatePeriodsUsableByTimeInAdvances(
        IEnumerable<PeriodOccupancyModel> models,
        TimeInAdvance timeInAdvance,
        DateTime currentDateTime)
    {
        foreach (var model in models.Where(x =>
                     x.AvailableVolume > 0 && !timeInAdvance.CanOccupy(x.GetStartingDateTime(), currentDateTime)))
        {
            model.AvailableVolume = 0;
        }
    }
}