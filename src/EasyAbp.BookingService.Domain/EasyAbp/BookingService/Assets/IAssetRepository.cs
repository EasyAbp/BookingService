using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.Assets;

public interface IAssetRepository : IRepository<Asset, Guid>
{
}