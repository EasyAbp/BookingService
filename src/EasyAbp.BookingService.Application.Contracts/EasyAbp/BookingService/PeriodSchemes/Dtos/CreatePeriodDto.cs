using System;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class CreatePeriodDto : ExtensibleObject, IHasPeriodInfo
{
    // TODO validate this should less than 24 hours
    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    /// <summary>
    /// If you set it to <c>true</c>, this period could only be fully occupied.
    /// For example, given the period is from 10:00-11:00.
    /// You can occupy 10:00-10:30 when set to <c>true</c> but cannot when set to <c>false</c>.
    /// </summary>
    public bool Divisible { get; set; }
}