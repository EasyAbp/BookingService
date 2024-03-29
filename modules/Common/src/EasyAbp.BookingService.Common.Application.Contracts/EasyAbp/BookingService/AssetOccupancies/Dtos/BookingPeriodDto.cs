﻿using System;

namespace EasyAbp.BookingService.AssetOccupancies.Dtos;

[Serializable]
public class BookingPeriodDto
{
    public DateTime Date { get; set; }

    public TimeSpan StartingTime { get; set; }

    public TimeSpan EndingTime { get; set; }

    public Guid PeriodSchemeId { get; set; }

    public Guid PeriodId { get; set; }

    public int TotalVolume { get; set; }

    public int AvailableVolume { get; set; }

    public TimeSpan Duration => EndingTime - StartingTime;
}