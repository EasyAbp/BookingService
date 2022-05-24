using System;
using System.Collections.Generic;
using System.Linq;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.PeriodSchemes;

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

    public List<BookablePeriod> CalculateBookablePeriods(DateTime searchDate, PeriodScheme periodScheme)
    {
        switch (PeriodUsable)
        {
            case PeriodUsable.Accept:
                // when a period is not divisible, and is fully contained in current schedule, it is bookable.  
                var bookablePeriods = periodScheme.Periods
                    .Where(x => !x.Divisible)
                    .Where(x => ContainsTimeRange(searchDate + x.StartingTime, searchDate + x.GetEndingTime()))
                    .Select(period => new BookablePeriod
                    {
                        StartingTime = period.StartingTime,
                        EndingTime = period.GetEndingTime(),
                        Divisible = period.Divisible,
                        PeriodId = period.Id,
                        PeriodSchemeId = periodScheme.Id
                    })
                    .Distinct()
                    .ToList();

                // when a period is divisible, and is intersected with current schedule's time range, it is partially bookable.
                var scheduleStartingTime = StartingDateTime - searchDate;
                var scheduleEndingTime = EndingDateTime - searchDate;
                var divisiblePeriods = new List<BookablePeriod>();
                foreach (var period in periodScheme.Periods
                             .Where(x => x.Divisible)
                             .Where(x => IsTimeRangeIntersected(searchDate + x.StartingTime,
                                 searchDate + x.GetEndingTime())))
                {
                    // this is the part of period, which is intersected with current schedule's time range
                    var periodStartingTime = scheduleStartingTime < period.StartingTime
                        ? period.StartingTime
                        : scheduleStartingTime;
                    var periodEndingTime = scheduleEndingTime < period.GetEndingTime()
                        ? scheduleEndingTime
                        : period.GetEndingTime();

                    divisiblePeriods.Add(new BookablePeriod
                    {
                        StartingTime = periodStartingTime,
                        EndingTime = periodEndingTime,
                        Divisible = period.Divisible,
                        PeriodId = period.Id,
                        PeriodSchemeId = periodScheme.Id
                    });
                }

                bookablePeriods.AddRange(divisiblePeriods);
                return bookablePeriods;
            case PeriodUsable.Reject:
                return new List<BookablePeriod>();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}