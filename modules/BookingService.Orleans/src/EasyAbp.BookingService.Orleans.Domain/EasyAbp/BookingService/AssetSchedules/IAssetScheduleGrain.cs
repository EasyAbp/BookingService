using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace EasyAbp.BookingService.AssetSchedules;

public interface IAssetScheduleGrain : IGrainWithGuidCompoundKey
{
    Task<List<IAssetSchedule>> GetListAsync(Guid periodSchemeId);
    Task AddOrUpdateAsync(AssetScheduleGrainStateModel state);
    Task DeleteAsync(AssetScheduleGrainStateModel state);
}