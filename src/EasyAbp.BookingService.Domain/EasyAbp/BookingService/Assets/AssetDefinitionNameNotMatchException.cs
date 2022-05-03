using Volo.Abp;

namespace EasyAbp.BookingService.Assets;

public class AssetDefinitionNameNotMatchException : BusinessException
{
    public AssetDefinitionNameNotMatchException(string assetDefinitionName, string categoryAssetDefinitionName)
        : base(BookingServiceErrorCodes
            .AssetDefinitionNameNotMatch)
    {
        WithData(nameof(assetDefinitionName), assetDefinitionName);
        WithData(nameof(categoryAssetDefinitionName), categoryAssetDefinitionName);
    }
}