﻿using System;
using EasyAbp.BookingService.Assets.Dtos;
using EasyAbp.BookingService.Dtos;
using JetBrains.Annotations;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.BookingService.AssetSchedules.Dtos;

[Serializable]
public class CreateAssetScheduleDto : ExtensibleObject
{
    public DateTime Date { get; set; }

    public Guid AssetId { get; set; }

    public Guid PeriodSchemeId { get; set; }

    public Guid PeriodId { get; set; }

    /// <summary>
    /// Accept or reject occupying within this time frame.
    /// </summary>
    public PeriodUsable PeriodUsable { get; set; }

    /// <summary>
    /// This value object describes the time range for assets that can occupy.
    /// Will fall back to <see cref="AssetDto"/> if the value here is <c>null</c>.
    /// </summary>
    [CanBeNull]
    public TimeInAdvanceDto TimeInAdvance { get; set; }
}