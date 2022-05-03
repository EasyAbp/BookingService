using System;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.BookingService.PeriodSchemes;

public class Period : Entity<Guid>, IHasPeriodInfo
{
    public virtual TimeSpan StartingTime { get; protected set; }

    public virtual TimeSpan Duration { get; protected set; }

    /// <summary>
    /// If you set it to <c>true</c>, this period could only be fully occupied.
    /// For example, given the period is from 10:00-11:00.
    /// You can occupy 10:00-10:30 when set to <c>true</c> but cannot when set to <c>false</c>.
    /// </summary>
    public virtual bool Divisible { get; protected set; }

    protected Period()
    {
    }

    public Period(Guid id, TimeSpan startingTime, TimeSpan duration, bool divisible) : base(id)
    {
        StartingTime = startingTime;
        Duration = duration;
        Divisible = divisible;
    }

    public void Update(TimeSpan startingTime, TimeSpan duration, bool divisible)
    {
        StartingTime = startingTime;
        Duration = duration;
        Divisible = divisible;
    }
}