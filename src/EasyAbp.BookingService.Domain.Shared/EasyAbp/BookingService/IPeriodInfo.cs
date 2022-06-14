using System;

namespace EasyAbp.BookingService;

public interface IPeriodInfo
{
    TimeSpan StartingTime { get; }
    
    TimeSpan Duration { get; }
}