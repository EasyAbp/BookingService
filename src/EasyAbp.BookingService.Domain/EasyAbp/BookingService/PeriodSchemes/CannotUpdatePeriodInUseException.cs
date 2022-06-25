using System;
using Volo.Abp;

namespace EasyAbp.BookingService.PeriodSchemes;

public class CannotUpdatePeriodInUseException : BusinessException
{
    public CannotUpdatePeriodInUseException(Guid id) : base(BookingServiceErrorCodes.CannotUpdatePeriodInUse)
    {
        WithData(nameof(id), id);
    }
}