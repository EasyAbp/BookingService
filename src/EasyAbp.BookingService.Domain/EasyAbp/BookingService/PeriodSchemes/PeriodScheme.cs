using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodScheme : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual string Name { get; protected set; }

    /// <summary>
    /// Cannot delete scheme with this property is set to <c>true</c>.
    /// </summary>
    public virtual bool IsDefault { get; protected set; }

    public virtual List<Period> Periods { get; protected set; }

    protected PeriodScheme()
    {
        Periods = new List<Period>();
    }

    public PeriodScheme(Guid id, Guid? tenantId, string name, bool isDefault, List<Period> periods) : base(id)
    {
        TenantId = tenantId;
        Name = name;
        IsDefault = isDefault;

        Periods = periods ?? new List<Period>();
    }

    public void UpdateIsDefault(bool v)
    {
        IsDefault = v;
    }

    public void Update(string name, List<Period> periods)
    {
        Name = name;
        Periods = periods;
    }

    public TimeSpan GetLatestEndingTime(PeriodScheme effectivePeriodScheme)
    {
        TimeSpan endingTime;
        if (effectivePeriodScheme.Periods.IsNullOrEmpty())
        {
            endingTime = TimeSpan.FromDays(1);
        }
        else
        {
            endingTime = effectivePeriodScheme.Periods
                .Select(x => x.GetEndingTime())
                .OrderByDescending(x => x)
                .First();

            if (endingTime < TimeSpan.FromDays(1))
            {
                endingTime = TimeSpan.FromDays(1);
            }
        }

        return endingTime;
    }
}