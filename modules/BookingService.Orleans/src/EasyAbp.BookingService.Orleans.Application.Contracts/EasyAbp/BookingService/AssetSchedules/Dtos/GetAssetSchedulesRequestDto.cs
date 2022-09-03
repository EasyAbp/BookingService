using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetSchedules.Dtos;

[Serializable]
public class GetAssetSchedulesRequestDto : PagedAndSortedResultRequestDto
{
    public DateTime? Date { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? AssetCategoryId { get; set; }
}