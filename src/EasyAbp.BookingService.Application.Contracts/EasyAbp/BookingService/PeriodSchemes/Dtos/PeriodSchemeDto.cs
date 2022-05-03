using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.PeriodSchemes.Dtos;

[Serializable]
public class PeriodSchemeDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public bool IsDefault { get; set; }

    public List<PeriodDto> Periods { get; set; }
}