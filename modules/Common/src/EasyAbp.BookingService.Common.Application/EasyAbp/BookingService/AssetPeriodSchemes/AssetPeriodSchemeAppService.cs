using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetPeriodSchemes;

public class AssetPeriodSchemeAppService : AbstractKeyCrudAppService<AssetPeriodScheme, AssetPeriodSchemeDto,
        AssetPeriodSchemeKey, GetAssetPeriodSchemesRequestDto, CreateAssetPeriodSchemeDto,
        UpdateAssetPeriodSchemeDto>,
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

    protected override Task DeleteByIdAsync(AssetPeriodSchemeKey id)
    {
        return _repository.DeleteAsync(x => x.AssetId == id.AssetId && x.Date == id.Date);
    }

    protected override Task<AssetPeriodScheme> GetEntityByIdAsync(AssetPeriodSchemeKey id)
    {
        return _repository.GetAsync(x => x.AssetId == id.AssetId && x.Date == id.Date);
    }

    protected override async Task<IQueryable<AssetPeriodScheme>> CreateFilteredQueryAsync(
        GetAssetPeriodSchemesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query.WhereIf(input.PeriodSchemeId.HasValue,
                x => x.PeriodSchemeId == input.PeriodSchemeId.Value)
            .WhereIf(input.AssetId.HasValue, x => x.AssetId == input.AssetId.Value)
            .WhereIf(input.Date.HasValue, x => x.Date == input.Date.Value);
    }
}