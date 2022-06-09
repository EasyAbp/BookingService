using System;
using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class CreatePeriodSchemeDto : ExtensibleObject
{
    public string Name { get; set; }

    public List<CreateUpdatePeriodDto> Periods { get; set; } = new();
}