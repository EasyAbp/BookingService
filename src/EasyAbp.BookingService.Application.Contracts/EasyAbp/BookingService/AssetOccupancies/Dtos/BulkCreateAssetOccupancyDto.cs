using System;
using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

public class BulkCreateAssetOccupancyDto : ExtensibleObject
{
    public Guid? OccupierUserId { get; set; }

    public List<OccupyAssetInfoModel> Models { get; set; }
    
    public List<OccupyAssetByCategoryInfoModel> ByCategoryModels { get; set; }
}