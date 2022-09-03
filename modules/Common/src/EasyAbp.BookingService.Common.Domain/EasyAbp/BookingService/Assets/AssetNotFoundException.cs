using System;
using Volo.Abp;

namespace EasyAbp.BookingService.Assets;

public class AssetNotFoundException : BusinessException
{
    public AssetNotFoundException(Guid assetId)
        : base(BookingServiceErrorCodes
            .AssetNotExists)
    {
        WithData(nameof(assetId), assetId);
    }
}