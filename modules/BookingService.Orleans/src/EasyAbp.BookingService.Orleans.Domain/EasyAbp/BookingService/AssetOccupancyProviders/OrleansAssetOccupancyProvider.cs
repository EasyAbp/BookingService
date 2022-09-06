using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules;
using Orleans;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

[ExposeServices(typeof(IAssetOccupancyProvider), typeof(OrleansAssetOccupancyProvider))]
[Dependency(ReplaceServices = true)]
public class OrleansAssetOccupancyProvider : AssetOccupancyProviderBase, ITransientDependency
{
    private readonly IGrainFactory _grainFactory;

    public OrleansAssetOccupancyProvider(IServiceProvider serviceProvider,
        IGrainFactory grainFactory) : base(serviceProvider)
    {
        _grainFactory = grainFactory;
    }

    protected override async Task<List<ProviderAssetOccupancyModel>> ProviderGetAssetOccupanciesAsync(
        DateTime targetDate,
        Guid assetId)
    {
        var grain = await GetGrainAsync(assetId, targetDate);
        return await grain.GetAssetOccupanciesAsync();
    }

    protected override async Task<ProviderAssetOccupancyModel> ProviderOccupyAsync(ProviderOccupyingInfoModel model)
    {
        var grain = await GetGrainAsync(model.Asset.Id, model.Date);
        return await grain.OccupyAsync(model);
    }

    protected override async Task<bool> ProviderTryRollBackOccupancyAsync(ProviderAssetOccupancyModel model)
    {
        var grain = await GetGrainAsync(model.AssetId, model.Date);
        return await grain.TryRollBackOccupancyAsync(model);
    }

    protected virtual Task<IAssetOccupancyGrain> GetGrainAsync(Guid assetId, DateTime date)
    {
        return Task.FromResult(
            _grainFactory.GetGrain<IAssetOccupancyGrain>(assetId, CalculateCompoundKey(date)));
    }

    protected virtual string CalculateCompoundKey(DateTime date)
    {
        return AssetScheduleExtensions.CalculateCompoundKey(date, CurrentTenant.Id);
    }
}