using System;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.BookingService.PeriodSchemes;

public class Period : Entity<Guid>, IPeriodInfo
{
    public virtual TimeSpan StartingTime { get; protected set; }

    public virtual TimeSpan Duration { get; protected set; }

    protected Period()
    {
    }

    internal Period(Guid id, TimeSpan startingTime, TimeSpan duration) : base(id)
    {
        StartingTime = startingTime;
        Duration = duration;
    }

    internal void Update(TimeSpan startingTime, TimeSpan duration)
    {
        StartingTime = startingTime;
        Duration = duration;
    }
}