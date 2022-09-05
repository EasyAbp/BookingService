using Volo.Abp;

namespace EasyAbp.BookingService.PeriodSchemes;

public class DefaultPeriodSchemeNotFoundException : BusinessException
{
    public DefaultPeriodSchemeNotFoundException() : base(BookingServiceErrorCodes
        .DefaultPeriodSchemeNotFound)
    {
    }
}