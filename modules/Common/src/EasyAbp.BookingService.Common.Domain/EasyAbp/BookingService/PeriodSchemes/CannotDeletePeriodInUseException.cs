using System;
using Volo.Abp;

namespace EasyAbp.BookingService.PeriodSchemes;

public class CannotDeletePeriodInUseException : BusinessException
{
    public CannotDeletePeriodInUseException(Guid id) : base(BookingServiceErrorCodes.CannotDeletePeriodInUse)
    {
        WithData(nameof(id), id);
    }
}