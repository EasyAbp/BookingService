using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetCategories.Dtos;

[Serializable]
public class GetAssetCategoriesRequestDto : PagedAndSortedResultRequestDto
{
    public string AssetDefinitionName { get; set; }

    public bool? Disabled { get; set; }

    public string DisplayName { get; set; }
    
    public Guid? ParentId { get; set; }
}