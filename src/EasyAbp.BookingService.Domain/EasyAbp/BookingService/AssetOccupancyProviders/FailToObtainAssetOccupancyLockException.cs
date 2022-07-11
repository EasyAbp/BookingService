﻿using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.PeriodSchemes;
using Volo.Abp;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class FailToObtainAssetOccupancyLockException : BusinessException
{
    public FailToObtainAssetOccupancyLockException(AssetCategory category, PeriodScheme periodScheme,
        IOccupyingTimeInfo model, int timeoutSeconds)
        : base(BookingServiceErrorCodes.FailToObtainAssetOccupancyLock)
    {
        WithData("categoryId", category.Id);
        WithData("periodSchemeId", periodScheme.Id);
        WithData("date", model.Date);
        WithData("timeout", timeoutSeconds);
    }
}