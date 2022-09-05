using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class GetAssetOccupanciesRequestDto : PagedAndSortedResultRequestDto
{
    public Guid? AssetId { get; set; }

    public DateTime? Date { get; set; }
}