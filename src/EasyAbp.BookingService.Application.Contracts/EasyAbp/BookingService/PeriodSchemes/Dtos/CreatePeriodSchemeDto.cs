using System;
using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class CreatePeriodSchemeDto : ExtensibleObject
{
    public string Name { get; set; }

    public bool IsDefault { get; set; }

    public List<CreatePeriodDto> Periods { get; set; }
}