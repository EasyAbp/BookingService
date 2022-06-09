using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class PeriodDto : EntityDto<Guid>, IHasPeriodInfo
{
    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }
}