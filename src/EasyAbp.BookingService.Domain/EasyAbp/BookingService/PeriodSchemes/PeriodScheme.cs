using System;
using System.Collections.Generic;
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
}