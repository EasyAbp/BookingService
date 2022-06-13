using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class AssetOccupancyDto : ExtensibleCreationAuditedEntityDto<Guid>, IHasPeriodInfo
{
    public Guid AssetId { get; set; }

    public string Asset { get; set; }

    public string AssetDefinitionName { get; set; }

    public int Volume { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? OccupierUserId { get; set; }

    public string OccupierName { get; set; }
}