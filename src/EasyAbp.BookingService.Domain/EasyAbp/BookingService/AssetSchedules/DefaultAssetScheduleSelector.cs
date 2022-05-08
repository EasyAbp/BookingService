using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.BookingService.AssetSchedules;

public class DefaultAssetScheduleSelector : IAssetScheduleSelector, ITransientDependency
{
    private readonly ILogger<DefaultAssetScheduleSelector> _logger;

    public DefaultAssetScheduleSelector(ILogger<DefaultAssetScheduleSelector> logger)
    {
        _logger = logger;
    }

    public virtual Task<AssetSchedule> SelectAsync(List<AssetSchedule> assetSchedules,
        DateTime date, TimeSpan startingTime, TimeSpan duration)
    {
        // TODO 部分满足是否属于该排期?
        assetSchedules.RemoveAll(x => !x.ContainsTimeRange(date, startingTime, duration));

        // reject policy's priority is higher
        assetSchedules = assetSchedules.GroupBy(x => x.SchedulePolicy)
            .OrderByDescending(x => x.Key)
            .Select(grouping => grouping.ToList())
            .FirstOrDefault() ?? new List<AssetSchedule>();

        if (assetSchedules.Count > 1)
        {
            _logger.LogWarning(
                "Multiple Asset Schedule is matched, with time range: {Date} {StartingTime}, duration: {Duration}",
                date, startingTime, duration);
            // TODO we should log warning or throw ex?
            // throw new MultipleAssetScheduleMatchedException(assetId, date, startingTime, duration);
        }

        return Task.FromResult(assetSchedules.FirstOrDefault());
    }
}