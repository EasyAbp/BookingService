using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.Assets;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public interface IAssetInCategorySelector
{
    Task<List<Asset>> SortAsync(List<Asset> assets);
}