using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.PeriodSchemes;

public class PeriodSchemeAppService : CrudAppService<PeriodScheme, PeriodSchemeDto, Guid, GetPeriodSchemesRequestDto
        , CreatePeriodSchemeDto, UpdatePeriodSchemeDto>,
    IPeriodSchemeAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.PeriodScheme.Delete;

    private readonly IPeriodSchemeRepository _repository;

    public PeriodSchemeAppService(IPeriodSchemeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<PeriodScheme>> CreateFilteredQueryAsync(
        GetPeriodSchemesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query
            .WhereIf(!input.Name.IsNullOrWhiteSpace(),
                x => x.Name == input.Name);
    }
}