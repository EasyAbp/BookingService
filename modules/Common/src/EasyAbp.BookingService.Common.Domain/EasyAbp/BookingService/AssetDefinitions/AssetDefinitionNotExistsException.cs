using Volo.Abp;

namespace EasyAbp.BookingService.AssetDefinitions;

public class AssetDefinitionNotExistsException : BusinessException
{
    public AssetDefinitionNotExistsException(string assetDefinitionName) : base(BookingServiceErrorCodes
        .AssetDefinitionNotExists)
    {
        WithData(nameof(assetDefinitionName), assetDefinitionName);
    }
}