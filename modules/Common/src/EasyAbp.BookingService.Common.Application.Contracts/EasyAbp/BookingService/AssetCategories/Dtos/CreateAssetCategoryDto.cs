using System;

namespace EasyAbp.BookingService.AssetCategories.Dtos;

[Serializable]
public class CreateAssetCategoryDto : UpdateAssetCategoryDto
{
    public string AssetDefinitionName { get; set; }
}