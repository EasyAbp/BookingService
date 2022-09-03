using System;
using Volo.Abp;

namespace EasyAbp.BookingService.PeriodSchemes;

public class CannotDeletePeriodSchemeInUseException : BusinessException
{
    public CannotDeletePeriodSchemeInUseException(Guid id, string name) : base(BookingServiceErrorCodes
        .CannotDeletePeriodSchemeInUse)
    {
        WithData(nameof(id), id);
        WithData(nameof(name), name);
    }
}