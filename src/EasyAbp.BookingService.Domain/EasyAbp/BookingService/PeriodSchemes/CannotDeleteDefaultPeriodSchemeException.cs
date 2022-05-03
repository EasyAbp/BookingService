using Volo.Abp;

namespace EasyAbp.BookingService.PeriodSchemes;

public class CannotDeleteDefaultPeriodSchemeException : BusinessException
{
    public CannotDeleteDefaultPeriodSchemeException(string name)
        : base(BookingServiceErrorCodes
            .CanNotDeleteDefaultPeriodScheme)
    {
        WithData(nameof(name), name);
    }
}