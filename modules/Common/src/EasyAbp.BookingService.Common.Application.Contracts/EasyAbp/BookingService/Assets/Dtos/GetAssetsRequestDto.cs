using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.Assets.Dtos;

[Serializable]
public class GetAssetsRequestDto : PagedAndSortedResultRequestDto
{
    public string Name { get; set; }

    public string AssetDefinitionName { get; set; }

    public Guid? AssetCategoryId { get; set; }

    public bool? Disabled { get; set; }
}