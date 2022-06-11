using System;
using Volo.Abp;

namespace EasyAbp.BookingService.AssetSchedules;

public class AssetScheduleExistsException : BusinessException
{
    public AssetScheduleExistsException(DateTime date, Guid assetId, Guid periodSchemeId, Guid periodId) : base(
        BookingServiceErrorCodes.AssetScheduleExists)
    {
        WithData(nameof(date), date);
        WithData(nameof(assetId), assetId);
        WithData(nameof(periodSchemeId), periodSchemeId);
        WithData(nameof(periodId), periodId);
    }
}