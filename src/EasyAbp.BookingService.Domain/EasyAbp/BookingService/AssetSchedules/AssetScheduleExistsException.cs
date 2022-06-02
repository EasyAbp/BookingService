using System;
using Volo.Abp;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleExistsException : BusinessException
{
    public AssetScheduleExistsException(Guid assetId, DateTime startingDateTime, DateTime endingDateTime,
        PeriodUsable periodUsable)
        : base(BookingServiceErrorCodes.AssetScheduleExists)
    {
        WithData(nameof(assetId), assetId);
        WithData(nameof(startingDateTime), startingDateTime);
        WithData(nameof(endingDateTime), endingDateTime);
        WithData(nameof(periodUsable), periodUsable);
    }
}