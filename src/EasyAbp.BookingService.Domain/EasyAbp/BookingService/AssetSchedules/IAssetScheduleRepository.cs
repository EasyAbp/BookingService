using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleRepository : IRepository<AssetSchedule, Guid>
{
}