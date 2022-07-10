using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.Assets;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class DefaultAssetInCategorySelector : IAssetInCategorySelector, ITransientDependency
{
    public virtual Task<List<Asset>> SelectAsync(List<Asset> assets)
    {
        var result = new List<Asset>();

        foreach (var assetGroup in assets
                     .GroupBy(x => x.Priority)
                     .OrderByDescending(x => x.Key))
        {
            result.AddRange(RandomHelper.GenerateRandomizedList(assetGroup));
        }

        return Task.FromResult(result);
    }
}