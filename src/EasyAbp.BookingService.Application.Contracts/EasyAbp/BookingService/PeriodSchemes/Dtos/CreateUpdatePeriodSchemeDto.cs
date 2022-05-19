using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class CreateUpdatePeriodSchemeDto : ExtensibleObject
{
    public string Name { get; set; }

    public List<CreatePeriodDto> Periods { get; set; } = new();
}