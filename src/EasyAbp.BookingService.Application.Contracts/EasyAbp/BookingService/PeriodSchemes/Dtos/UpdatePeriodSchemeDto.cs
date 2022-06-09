using System;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class UpdatePeriodSchemeDto : ExtensibleObject
{
    public string Name { get; set; }
}