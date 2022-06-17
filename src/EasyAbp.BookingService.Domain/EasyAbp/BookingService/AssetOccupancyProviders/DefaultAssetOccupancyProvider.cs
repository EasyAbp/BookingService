using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancyCounts;
using EasyAbp.BookingService.Assets;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class DefaultAssetOccupancyProvider : AssetOccupancyProviderBase, ITransientDependency
{
    private readonly IAssetOccupancyCountRepository _assetOccupancyCountRepository;

    public DefaultAssetOccupancyProvider(
        IAssetOccupancyCountRepository assetOccupancyCountRepository,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _assetOccupancyCountRepository = assetOccupancyCountRepository;
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
    protected override async Task<ProviderAssetOccupancyModel> ProviderOccupyAsync(Asset asset,
        AssetCategory category, OccupyAssetInfoModel model)
    {
        var occupancyCount =
            await _assetOccupancyCountRepository.FindAsync(new AssetOccupancyCountKey(model.Date, model.AssetId,
                model.StartingTime, model.Duration));

        if (occupancyCount is null)
        {
            occupancyCount = await _assetOccupancyCountRepository.InsertAsync(
                new AssetOccupancyCount(CurrentTenant.Id, asset.Id,
                    $"{category.DisplayName}-{asset.Name}", model.Date, model.StartingTime,
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