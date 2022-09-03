using Volo.Abp;

namespace EasyAbp.BookingService.PeriodSchemes;

public class DefaultPeriodSchemeAlreadyExistsException : BusinessException
{
    public DefaultPeriodSchemeAlreadyExistsException() : base(BookingServiceErrorCodes
        .DefaultPeriodSchemeAlreadyExists)
    {
    }
}