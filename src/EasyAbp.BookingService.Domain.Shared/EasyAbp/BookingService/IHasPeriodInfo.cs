using System;

namespace EasyAbp.BookingService;

public interface IHasPeriodInfo
{
    TimeSpan StartingTime { get; }
    
    TimeSpan Duration { get; }
}