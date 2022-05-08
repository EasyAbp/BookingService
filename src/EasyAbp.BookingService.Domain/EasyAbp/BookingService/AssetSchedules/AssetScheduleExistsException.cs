using System;
using Volo.Abp;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleExistsException : BusinessException
{
    public AssetScheduleExistsException(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration)
        : base(BookingServiceErrorCodes
            .AssetScheduleExists)
    {
        WithData(nameof(assetId), assetId);
        WithData(nameof(date), date);
        WithData(nameof(startingTime), startingTime);
        WithData(nameof(duration), duration);
    }
}