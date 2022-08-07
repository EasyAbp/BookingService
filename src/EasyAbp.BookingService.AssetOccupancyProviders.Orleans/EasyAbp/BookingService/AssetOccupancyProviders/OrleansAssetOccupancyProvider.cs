using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies;
using Orleans;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Uow;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

[ExposeServices(typeof(IAssetOccupancyProvider), typeof(OrleansAssetOccupancyProvider))]
[Dependency(ReplaceServices = true)]
public class OrleansAssetOccupancyProvider : AssetOccupancyProviderBase, ITransientDependency
{
    private readonly IGrainFactory _grainFactory;
    private readonly IAbpDistributedLock _distributedLock;
    private const string BulkOccupancyLock = nameof(BulkOccupancyLock);

    public OrleansAssetOccupancyProvider(IServiceProvider serviceProvider,
        IGrainFactory grainFactory,
        IAbpDistributedLock distributedLock) : base(serviceProvider)
    {
        _grainFactory = grainFactory;
        _distributedLock = distributedLock;
    }

    public override async Task<List<(ProviderAssetOccupancyModel, AssetOccupancy)>> BulkOccupyAsync(
        List<OccupyAssetInfoModel> models, List<OccupyAssetByCategoryInfoModel> byCategoryModels, Guid? occupierUserId)
    {
        using var uow = UnitOfWorkManager.Begin(true, true);

        await using var handle = await _distributedLock.TryAcquireAsync(BulkOccupancyLock,
            TimeSpan.FromSeconds(Options.AssetOccupyLockTimeoutSeconds));

        if (handle is null)
        {
            throw new FailToObtainAssetOccupancyLockException();
        }

        var result = await CanBulkOccupyAsync(models, byCategoryModels);
        await HandleCanOccupyResultAsync(result);

        var assetOccupancies = await InternalBulkOccupyAsync(models, byCategoryModels, occupierUserId);

        await uow.CompleteAsync();
        return assetOccupancies;
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
        return await grain.OccupyAsync(model, CurrentTenant.Id);
    }

    protected override async Task<bool> ProviderTryRollBackOccupancyAsync(ProviderAssetOccupancyModel model)
    {
        var grain = await GetGrainAsync(model.AssetId, model.Date);
        return await grain.TryRollBackOccupancyAsync(model);
    }

    protected virtual Task<IAssetOccupancyGrain> GetGrainAsync(Guid assetId, DateTime date)
    {
        return Task.FromResult(
            _grainFactory.GetGrain<IAssetOccupancyGrain>(assetId, date.ToString(DateTimeFormatInfo.InvariantInfo)));
    }
}