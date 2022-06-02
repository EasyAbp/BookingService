using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetOccupancies;

public interface IAssetOccupancyRepository : IRepository<AssetOccupancy, Guid>
{
}