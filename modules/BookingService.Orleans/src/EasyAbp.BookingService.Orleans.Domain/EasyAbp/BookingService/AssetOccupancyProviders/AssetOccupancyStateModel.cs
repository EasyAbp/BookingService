using System;
using System.Collections.Generic;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

[Serializable]
public class AssetOccupancyStateModel
{
    public List<AssetOccupancyCountModel> AssetOccupancies { get; set; } = new();
}