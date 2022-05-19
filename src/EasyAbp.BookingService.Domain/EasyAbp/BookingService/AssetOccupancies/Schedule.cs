using System;
using System.Collections.Generic;
using System.Linq;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.PeriodSchemes;

namespace EasyAbp.BookingService.AssetOccupancies;

public class Schedule
{
    public DateTime StartingDateTime { get; set; }

    public DateTime EndingDateTime { get; set; }

    public PeriodUsable PeriodUsable { get; set; }

    public TimeInAdvance TimeInAdvance { get; set; }

    public bool IsTimeRangeIntersected(DateTime startingDateTime, DateTime endingDateTime)
    {
        return !(startingDateTime >= EndingDateTime || endingDateTime <= StartingDateTime);
    }

    public bool IsContainsTimeRange(DateTime startingDateTime, DateTime endingDateTime)
    {
        return StartingDateTime <= startingDateTime && endingDateTime <= EndingDateTime;
    }

    public IEnumerable<Schedule> RemoveIntersectingTimeRange(DateTime startingDateTime, DateTime endingDateTime)
    {
        if (startingDateTime < StartingDateTime
            && StartingDateTime < endingDateTime
            && endingDateTime <= EndingDateTime)
        {
            yield return new Schedule
            {
                PeriodUsable = PeriodUsable,
                TimeInAdvance = TimeInAdvance,
                StartingDateTime = endingDateTime,
                EndingDateTime = EndingDateTime
            };
        }
        else if (StartingDateTime <= startingDateTime
                 && startingDateTime < EndingDateTime
                 && EndingDateTime < endingDateTime)
        {
            yield return new Schedule
            {
                PeriodUsable = PeriodUsable,
                TimeInAdvance = TimeInAdvance,
                StartingDateTime = StartingDateTime,
                EndingDateTime = startingDateTime
            };
        }
        else
        {
            yield return new Schedule
            {
                PeriodUsable = PeriodUsable,
                TimeInAdvance = TimeInAdvance,
                StartingDateTime = StartingDateTime,
                EndingDateTime = startingDateTime
            };

            yield return new Schedule
            {
                PeriodUsable = PeriodUsable,
                TimeInAdvance = TimeInAdvance,
                StartingDateTime = endingDateTime,
                EndingDateTime = EndingDateTime
            };
        }
    }

    public bool IsEmptyTimeRange()
    {
        return StartingDateTime == EndingDateTime;
    }

    public List<BookablePeriod> CalculateBookablePeriods(DateTime searchDate, List<Period> periods)
    {
        switch (PeriodUsable)
        {
            case PeriodUsable.Accept:
                var bookablePeriods = periods
                    .Where(x => !x.Divisible)
                    .Where(x => IsContainsTimeRange(searchDate + x.StartingTime, searchDate + x.GetEndingTime()))
                    .Select(period => new BookablePeriod
                    {
                        StartingTime = period.StartingTime,
                        EndingTime = period.GetEndingTime(),
                        Divisible = period.Divisible
                    })
                    .Distinct()
                    .ToList();

                var startingTime = StartingDateTime - searchDate;
                var endingTime = EndingDateTime - searchDate;
                var divisiblePeriods = new List<BookablePeriod>();
                foreach (var period in periods.Where(x => x.Divisible))
                {
                    var periodStartingTime = startingTime < period.StartingTime
                        ? period.StartingTime
                        : startingTime;
                    var periodEndingTime = endingTime < period.GetEndingTime()
                        ? endingTime
                        : period.GetEndingTime();
                    var bookablePeriod =
                        divisiblePeriods.FirstOrDefault(x =>
                            x.IsIntersected(periodStartingTime, periodEndingTime));
                    if (bookablePeriod is not null)
                    {
                        bookablePeriod.Merge(periodStartingTime, periodEndingTime);
                    }
                    else
                    {
                        divisiblePeriods.Add(new BookablePeriod
                        {
                            StartingTime = periodStartingTime,
                            EndingTime = periodEndingTime,
                            Divisible = period.Divisible
                        });
                    }
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