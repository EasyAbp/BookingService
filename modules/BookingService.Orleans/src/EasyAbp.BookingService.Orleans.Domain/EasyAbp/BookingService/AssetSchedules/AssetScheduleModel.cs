using System;

namespace EasyAbp.BookingService.AssetSchedules;

[Serializable]
public class AssetScheduleModel : IAssetSchedule
{
    public AssetScheduleModel(DateTime date, Guid assetId, Guid periodSchemeId, Guid periodId,
        PeriodUsable periodUsable, TimeInAdvance timeInAdvance)
    {
        Date = date;
        AssetId = assetId;
        PeriodSchemeId = periodSchemeId;
        PeriodId = periodId;
        PeriodUsable = periodUsable;
        TimeInAdvance = timeInAdvance;
    }

    public AssetScheduleModel()
    {
    }

    public DateTime Date { get; set; }
    public Guid AssetId { get; set; }
    public Guid PeriodSchemeId { get; set; }
    public Guid PeriodId { get; set; }
    public PeriodUsable PeriodUsable { get; set; }
    public TimeInAdvance TimeInAdvance { get; set; }
}