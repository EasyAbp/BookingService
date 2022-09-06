using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleGrain : Grain, IAssetScheduleGrain
{
    private readonly IAssetScheduleRepository _assetScheduleRepository;
    private readonly Dictionary<Guid, AssetScheduleGrainStateModel> _state = new();
    private Guid _assetId;
    private DateTime _date;

    public AssetScheduleGrain(IAssetScheduleRepository assetScheduleRepository)
    {
        _assetScheduleRepository = assetScheduleRepository;
    }

    public override async Task OnActivateAsync()
    {
        _assetId = this.GetPrimaryKey(out var compoundKey);
        _date = compoundKey.GetDateFromCompoundKey();

        // Read state from db
        var schedules = await _assetScheduleRepository.GetListAsync(x => x.AssetId == _assetId && x.Date == _date);
        foreach (var stateModel in ToStateModel(schedules))
        {
            _state[stateModel.Id] = stateModel;
        }
    }

    public virtual Task<List<IAssetSchedule>> GetListAsync(Guid periodSchemeId)
    {
        var list = _state.Values.Where(x => x.PeriodSchemeId == periodSchemeId)
            .Select(x => new AssetScheduleModel(
                _date,
                _assetId,
                x.PeriodSchemeId,
                x.PeriodId,
                x.PeriodUsable,
                x.TimeInAdvance))
            .Cast<IAssetSchedule>().ToList();
        return Task.FromResult(list);
    }

    public virtual Task AddOrUpdateAsync(AssetScheduleGrainStateModel state)
    {
        _state[state.Id] = state;
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(AssetScheduleGrainStateModel state)
    {
        if (_state.ContainsKey(state.Id))
        {
            _state.Remove(state.Id);
        }

        return Task.CompletedTask;
    }

    protected virtual IEnumerable<AssetScheduleGrainStateModel> ToStateModel(IEnumerable<AssetSchedule> schedules)
    {
        return schedules.Select(assetSchedule => assetSchedule.ToAssetScheduleGrainStateModel());
    }
}