using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public class AssetPeriodSchemeAppService : CrudAppService<AssetPeriodScheme, AssetPeriodSchemeDto,
        AssetPeriodSchemeKey, GetAssetPeriodSchemesRequestDto, CreateUpdateAssetPeriodSchemeDto,
        CreateUpdateAssetPeriodSchemeDto>,
    IAssetPeriodSchemeAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.AssetPeriodScheme.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.AssetPeriodScheme.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.AssetPeriodScheme.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.AssetPeriodScheme.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.AssetPeriodScheme.Delete;

    private readonly IAssetPeriodSchemeRepository _repository;

    public AssetPeriodSchemeAppService(IAssetPeriodSchemeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<AssetPeriodScheme>> CreateFilteredQueryAsync(
        GetAssetPeriodSchemesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query.WhereIf(input.PeriodSchemeId.HasValue,
            x => x.PeriodSchemeId == input.PeriodSchemeId.Value);
    }
}