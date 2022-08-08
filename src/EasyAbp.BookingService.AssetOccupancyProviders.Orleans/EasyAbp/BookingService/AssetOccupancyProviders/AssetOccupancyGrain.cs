using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancyCounts;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class AssetOccupancyGrain : Grain, IAssetOccupancyGrain
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AssetOccupancyGrain(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public virtual async Task<List<ProviderAssetOccupancyModel>> GetAssetOccupanciesAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var id = this.GetPrimaryKey(out var date);
        var assetOccupancyCountRepository =
            scope.ServiceProvider.GetRequiredService<IAssetOccupancyCountRepository>();
        var occupancies = await assetOccupancyCountRepository.GetListAsync(DateTime.Parse(date), id);

        return occupancies.Select(x =>
                new ProviderAssetOccupancyModel(x.AssetId, x.Volume, x.Date, x.StartingTime, x.Duration))
            .ToList();
    }

    public virtual async Task<ProviderAssetOccupancyModel> OccupyAsync(ProviderOccupyingInfoModel model,
        Guid? currentTenantId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var assetOccupancyCountRepository =
            scope.ServiceProvider.GetRequiredService<IAssetOccupancyCountRepository>();
        var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = unitOfWorkManager.Begin();

        var occupancyCount =
            await assetOccupancyCountRepository.FindAsync(new AssetOccupancyCountKey(model.Date, model.Asset.Id,
                model.StartingTime, model.Duration));

        if (occupancyCount is null)
        {
            occupancyCount = await assetOccupancyCountRepository.InsertAsync(
                new AssetOccupancyCount(currentTenantId, model.Asset.Id,
                    $"{model.CategoryOfAsset.DisplayName}-{model.Asset.Name}", model.Date, model.StartingTime,
                    model.Duration, model.Volume), true);
        }
        else
        {
            if (occupancyCount.Volume + model.Volume > model.Asset.Volume)
            {
                throw new InsufficientAssetVolumeException();
            }

            occupancyCount.ChangeVolume(model.Volume);
            await assetOccupancyCountRepository.UpdateAsync(occupancyCount, true);
        }

        await uow.CompleteAsync();

        return new ProviderAssetOccupancyModel(occupancyCount.AssetId, occupancyCount.Volume, occupancyCount.Date,
            occupancyCount.StartingTime, occupancyCount.Duration);
    }

    public virtual async Task<bool> TryRollBackOccupancyAsync(ProviderAssetOccupancyModel model)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var assetOccupancyCountRepository =
            scope.ServiceProvider.GetRequiredService<IAssetOccupancyCountRepository>();
        var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = unitOfWorkManager.Begin();

        try
        {
            var occupancyCount =
                await assetOccupancyCountRepository.FindAsync(new AssetOccupancyCountKey(model.Date, model.AssetId,
                    model.StartingTime, model.Duration));

            if (occupancyCount is null)
            {
                return false;
            }

            occupancyCount.ChangeVolume(-1 * model.Volume);

            await assetOccupancyCountRepository.UpdateAsync(occupancyCount, true);
            await uow.CompleteAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}