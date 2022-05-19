using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetSchedules.Dtos;

[Serializable]
public class GetAssetSchedulesRequestDto : PagedAndSortedResultRequestDto
{
    public Guid? AssetId { get; set; }

    public DateTime? StartingDateTime { get; set; }
    
    public DateTime? EndingDateTime { get; set; }
}