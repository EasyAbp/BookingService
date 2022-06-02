using System;
using System.Collections.Generic;
using EasyAbp.BookingService.AssetSchedules;

namespace EasyAbp.BookingService.AssetOccupancies;

/// <summary>
/// This is a calculation model for calculation process of BookablePeriod.
/// It stored effective values of PeriodUsable & TimeInAdvance in a time range plus some calculation method needed during calculation of book periods.
/// </summary>
public class EffectiveScheduleModel
{
    public EffectiveScheduleModel(DateTime startingDateTime,
        DateTime endingDateTime,
        PeriodUsable periodUsable,
        TimeInAdvance timeInAdvance)
    {
        StartingDateTime = startingDateTime;
        EndingDateTime = endingDateTime;
        PeriodUsable = periodUsable;
        TimeInAdvance = timeInAdvance;
    }

    public DateTime StartingDateTime { get; set; }

    public DateTime EndingDateTime { get; set; }

    public PeriodUsable PeriodUsable { get; set; }

    public TimeInAdvance TimeInAdvance { get; set; }

    public bool IsTimeRangeIntersected(DateTime targetStartingDateTime, DateTime targetEndingDateTime)
    {
        return !(targetStartingDateTime >= EndingDateTime || targetEndingDateTime <= StartingDateTime);
    }

    public bool ContainsTimeRange(DateTime targetStartingDateTime, DateTime targetEndingDateTime)
    {
        return StartingDateTime <= targetStartingDateTime && targetEndingDateTime <= EndingDateTime;
    }

    public IEnumerable<EffectiveScheduleModel> ExcludeIntersectedAndCreateNewSchedules(DateTime targetStartingDateTime,
        DateTime targetEndingDateTime)
    {
        if (targetStartingDateTime < StartingDateTime
            && StartingDateTime < targetEndingDateTime
            && targetEndingDateTime <= EndingDateTime)
        {
            yield return new EffectiveScheduleModel(targetEndingDateTime,
                EndingDateTime,
                PeriodUsable,
                TimeInAdvance);
        }
        else if (StartingDateTime <= targetStartingDateTime
                 && targetStartingDateTime < EndingDateTime
                 && EndingDateTime < targetEndingDateTime)
        {
            yield return new EffectiveScheduleModel(StartingDateTime,
                targetStartingDateTime,
                PeriodUsable,
                TimeInAdvance);
        }
        else
        {
            yield return new EffectiveScheduleModel(
                StartingDateTime,
                targetStartingDateTime,
                PeriodUsable,
                TimeInAdvance);

            yield return new EffectiveScheduleModel(
                targetEndingDateTime,
                EndingDateTime,
                PeriodUsable,
                TimeInAdvance);
        }
    }

    public bool IsEmptyTimeRange()
    {
        return StartingDateTime == EndingDateTime;
    }
}