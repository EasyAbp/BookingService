﻿using System;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class CategoryBookablePeriodDto : IHasPeriodInfo
{
    public Guid AssetId { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan Duration { get; set; }
}