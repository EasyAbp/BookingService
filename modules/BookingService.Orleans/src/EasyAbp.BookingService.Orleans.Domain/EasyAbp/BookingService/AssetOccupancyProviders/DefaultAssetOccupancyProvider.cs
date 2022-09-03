using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetOccupancyCounts;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class DefaultAssetOccupancyProvider : AssetOccupancyProviderBase, ITransientDependency
{
    protected const string AssetOccupancyLock = nameof(AssetOccupancyLock);
    private readonly IAssetOccupancyCountRepository _assetOccupancyCountRepository;
    private readonly IAbpDistributedLock _distributedLock;
    private readonly ILogger<DefaultAssetOccupancyProvider> _logger;

    public DefaultAssetOccupancyProvider(
        IAssetOccupancyCountRepository assetOccupancyCountRepository,
        IAbpDistributedLock distributedLock,
        ILogger<DefaultAssetOccupancyProvider> logger,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _assetOccupancyCountRepository = assetOccupancyCountRepository;
        _distributedLock = distributedLock;
        _logger = logger;
    }

    public override async Task<(ProviderAssetOccupancyModel, AssetOccupancy)> OccupyAsync(OccupyAssetInfoModel model,
        Guid? occupierUserId)
    {
        using var uow = UnitOfWorkManager.Begin(true, true);
        var asset = await AssetRepository.GetAsync(model.AssetId);
        var category = await AssetCategoryRepository.GetAsync(asset.AssetCategoryId);

        await using var handle = await _distributedLock.TryAcquireAsync(AssetOccupancyLock,
            TimeSpan.FromSeconds(Options.AssetOccupyLockTimeoutSeconds));
        if (handle is null)
        {
            throw new FailToObtainAssetOccupancyLockException();
        }

        var result = await OccupyAsync(asset, category, model, occupierUserId);
        await uow.CompleteAsync();
        return result;
    }

    public override async Task<(ProviderAssetOccupancyModel, AssetOccupancy)> OccupyByCategoryAsync(
        OccupyAssetByCategoryInfoModel model, Guid? occupierUserId)
    {
        using var uow = UnitOfWorkManager.Begin(true, true);
        var category = await AssetCategoryRepository.GetAsync(model.AssetCategoryId);
        if (category.Disabled)
        {
            throw new DisabledAssetOrCategoryException();
        }

        var assets =
            await AssetRepository.GetListAsync(x => x.AssetCategoryId == category.Id && !x.Disabled);

        await using var handle = await _distributedLock.TryAcquireAsync(AssetOccupancyLock,
            TimeSpan.FromSeconds(Options.AssetOccupyLockTimeoutSeconds));
        if (handle is null)
        {
            throw new FailToObtainAssetOccupancyLockException();
        }

        var result = await OccupyByCategoryAsync(category, assets, model, occupierUserId);
        await uow.CompleteAsync();
        return result;
    }

    public override async Task<List<(ProviderAssetOccupancyModel, AssetOccupancy)>> BulkOccupyAsync(
        List<OccupyAssetInfoModel> models, List<OccupyAssetByCategoryInfoModel> byCategoryModels, Guid? occupierUserId)
    {
        using var uow = UnitOfWorkManager.Begin(true, true);

        var assetSet = await CreateAssetSetAsync(models);
        var categorySet = await CreateCategorySetAsync(byCategoryModels);

        await using var handle = await _distributedLock.TryAcquireAsync(AssetOccupancyLock,
            TimeSpan.FromSeconds(Options.AssetOccupyLockTimeoutSeconds));

        if (handle is null)
        {
            throw new FailToObtainAssetOccupancyLockException();
        }

        var result = await CanBulkOccupyAsync(models, byCategoryModels);
        await HandleCanOccupyResultAsync(result);

        var assetOccupancies = new List<(ProviderAssetOccupancyModel, AssetOccupancy)>();

        try
        {
            foreach (var model in models)
            {
                var (asset, category) = assetSet[model.AssetId];
                assetOccupancies.Add(await OccupyAsync(asset, category, model, occupierUserId));
            }

            foreach (var model in byCategoryModels)
            {
                var (category, assets) = categorySet[model.AssetCategoryId];
                assetOccupancies.Add(await OccupyByCategoryAsync(category, assets, model, occupierUserId));
            }
        }
        catch
        {
            foreach (var (model, _) in assetOccupancies)
            {
                if (!await ProviderTryRollBackOccupancyAsync(model))
                {
                    _logger.LogWarning("Occupancy provider occupancy rollback failed! {Model}", model);
                }
            }

            throw;
        }

        await uow.CompleteAsync();
        return assetOccupancies;
    }

    [UnitOfWork]
    protected override async Task<List<ProviderAssetOccupancyModel>> ProviderGetAssetOccupanciesAsync(
        DateTime targetDate, Guid assetId)
    {
        var occupancies = await _assetOccupancyCountRepository.GetListAsync(targetDate, assetId);

        return occupancies.Select(x =>
                new ProviderAssetOccupancyModel(x.AssetId, x.Volume, x.Date, x.StartingTime, x.Duration))
            .ToList();
    }

    [UnitOfWork]
    protected override async Task<ProviderAssetOccupancyModel> ProviderOccupyAsync(ProviderOccupyingInfoModel model)
    {
        var occupancyCount =
            await _assetOccupancyCountRepository.FindAsync(new AssetOccupancyCountKey(model.Date, model.Asset.Id,
                model.StartingTime, model.Duration));

        if (occupancyCount is null)
        {
            occupancyCount = await _assetOccupancyCountRepository.InsertAsync(
                new AssetOccupancyCount(CurrentTenant.Id, model.Asset.Id,
                    $"{model.CategoryOfAsset.DisplayName}-{model.Asset.Name}", model.Date, model.StartingTime,
                    model.Duration, model.Volume), true);
        }
        else
        {
            occupancyCount.ChangeVolume(model.Volume);

            await _assetOccupancyCountRepository.UpdateAsync(occupancyCount, true);
        }

        return new ProviderAssetOccupancyModel(occupancyCount.AssetId, occupancyCount.Volume, occupancyCount.Date,
            occupancyCount.StartingTime, occupancyCount.Duration);
    }

    [UnitOfWork]
    protected override async Task<bool> ProviderTryRollBackOccupancyAsync(ProviderAssetOccupancyModel model)
    {
        if (UnitOfWorkManager.Current is not null && UnitOfWorkManager.Current.Options.IsTransactional)
        {
            return true;
        }

        try
        {
            var occupancyCount =
                await _assetOccupancyCountRepository.FindAsync(new AssetOccupancyCountKey(model.Date, model.AssetId,
                    model.StartingTime, model.Duration));

            if (occupancyCount is null)
            {
                return false;
            }

            occupancyCount.ChangeVolume(-1 * model.Volume);

            await _assetOccupancyCountRepository.UpdateAsync(occupancyCount, true);

            return true;
        }
        catch
        {
            return false;
        }
    }
}