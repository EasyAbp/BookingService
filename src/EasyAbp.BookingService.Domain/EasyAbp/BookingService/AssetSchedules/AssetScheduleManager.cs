using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleManager : DomainService, IAssetScheduleManager
{
    private readonly IAssetScheduleRepository _repository;

    public AssetScheduleManager(IAssetScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<AssetSchedule> CreateAsync(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance)
    {
        // TODO Do we need to check assetId exists?

        // TODO If there is multiple entities with same assetId & date & startingTime ?
        var assetSchedule = await _repository.FindAsync(x => x.AssetId == assetId
                                                             && x.Date == date
                                                             && x.StartingTime == startingTime);
        if (assetSchedule is not null)
        {
            // TODO If Asset Schedule is duplicated, throw Ex? 
        }
        
        throw new NotImplementedException();
    }

    public Task UpdateAsync(AssetSchedule entity, Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        AssetSchedulePolicy schedulePolicy, TimeInAdvance timeInAdvance)
    {
        throw new NotImplementedException();
    }
}