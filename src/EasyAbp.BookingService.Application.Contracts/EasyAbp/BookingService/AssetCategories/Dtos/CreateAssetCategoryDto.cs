using System;

namespace EasyAbp.BookingService.AssetCategories.Dtos;

[Serializable]
public class CreateAssetCategoryDto : UpdateAssetCategoryDto
{
    /// <summary>
    /// This should be readonly because Assets should have the same AssetDefinitionName as it's category.  
    /// </summary>
    public string AssetDefinitionName { get; set; }
}