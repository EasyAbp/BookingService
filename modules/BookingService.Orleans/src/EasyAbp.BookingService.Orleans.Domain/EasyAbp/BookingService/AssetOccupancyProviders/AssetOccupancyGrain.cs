using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetSchedules;
using Orleans;
using Orleans.Providers;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

[StorageProvider(ProviderName = StorageProviderName)]
public class AssetOccupancyGrain : Grain<AssetOccupancyStateModel>, IAssetOccupancyGrain
{
    public const string StorageProviderName = "BookingServiceAssetOccupancyStorage";

    private Guid _assetId;
    private DateTime _date;

    public override Task OnActivateAsync()
    {
        _assetId = this.GetPrimaryKey(out var compoundKey);
        _date = compoundKey.GetDateFromCompoundKey();
        return base.OnActivateAsync();
    }

    public virtual Task<List<ProviderAssetOccupancyModel>> GetAssetOccupanciesAsync()
    {
        return Task.FromResult(State.AssetOccupancies
            .Select(x => new ProviderAssetOccupancyModel(_assetId, x.Volume, _date, x.StartingTime, x.Duration))
            .ToList());
    }

    public virtual async Task<ProviderAssetOccupancyModel> OccupyAsync(ProviderOccupyingInfoModel model)
    {
        var assetOccupancyModel = State.AssetOccupancies.FirstOrDefault(x =>
            x.StartingTime == model.StartingTime && x.Duration == model.Duration);

        if (assetOccupancyModel is null)
        {
            assetOccupancyModel = new AssetOccupancyCountModel(model.StartingTime, model.Duration, model.Volume);
            State.AssetOccupancies.Add(assetOccupancyModel);
        }
        else
        {
            if (assetOccupancyModel.Volume + model.Volume > model.Asset.Volume)
            {
                throw new InsufficientAssetVolumeException();
            }

            if (!assetOccupancyModel.ChangeVolume(model.Volume))
            {
                throw new UnexpectedNegativeVolumeException(_assetId, _date, model.StartingTime, model.Duration,
                    assetOccupancyModel.Volume,
                    model.Volume);
            }
        }

        await WriteStateAsync();

        return new ProviderAssetOccupancyModel(_assetId, assetOccupancyModel.Volume, _date, model.StartingTime,
            model.Duration);
    }

    public virtual async Task<bool> TryRollBackOccupancyAsync(ProviderAssetOccupancyModel model)
    {
        try
        {
            var assetOccupancyModel = State.AssetOccupancies.FirstOrDefault(x =>
                x.StartingTime == model.StartingTime && x.Duration == model.Duration);

            if (assetOccupancyModel is null)
            {
                return false;
            }

            assetOccupancyModel.ChangeVolume(-1 * model.Volume);
            await WriteStateAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}