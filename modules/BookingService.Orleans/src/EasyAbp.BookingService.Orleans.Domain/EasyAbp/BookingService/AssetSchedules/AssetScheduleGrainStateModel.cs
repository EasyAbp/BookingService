using System;

namespace EasyAbp.BookingService.AssetSchedules;

[Serializable]
public class AssetScheduleGrainStateModel
{
    public AssetScheduleGrainStateModel(Guid id, Guid periodSchemeId, Guid periodId, PeriodUsable periodUsable,
        TimeInAdvance timeInAdvance)
    {
        Id = id;
        PeriodSchemeId = periodSchemeId;
        PeriodId = periodId;
        PeriodUsable = periodUsable;
        TimeInAdvance = timeInAdvance;
    }

    public AssetScheduleGrainStateModel()
    {
    }

    public Guid Id { get; set; }
    public Guid PeriodSchemeId { get; set; }
    public Guid PeriodId { get; set; }
    public PeriodUsable PeriodUsable { get; set; }
    public TimeInAdvance TimeInAdvance { get; set; }
}