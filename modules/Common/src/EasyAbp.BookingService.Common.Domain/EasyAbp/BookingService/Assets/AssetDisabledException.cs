using System;
using Volo.Abp;

namespace EasyAbp.BookingService.Assets;

public class AssetDisabledException : BusinessException
{
    public AssetDisabledException(Guid assetId)
        : base(BookingServiceErrorCodes.AssetDisabled)
    {
        WithData(nameof(assetId), assetId);
    }
}