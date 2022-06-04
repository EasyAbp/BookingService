using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyAppService : CrudAppService<AssetOccupancy, AssetOccupancyDto, Guid,
        GetAssetOccupanciesRequestDto, CreateAssetOccupancyDto>,
    IAssetOccupancyAppService
{
    protected override string GetPolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Default;
    protected override string GetListPolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Default;
    protected override string CreatePolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Create;
    protected override string UpdatePolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Update;
    protected override string DeletePolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Delete;
    protected virtual string SearchPolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Search;
    protected virtual string CheckPolicyName { get; set; } = BookingServicePermissions.AssetOccupancy.Check;

    private readonly IAssetOccupancyRepository _repository;
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly AssetOccupancyManager _assetOccupancyManager;

    public AssetOccupancyAppService(IAssetOccupancyRepository repository,
        IAssetRepository assetRepository,
        IAssetCategoryRepository assetCategoryRepository,
        AssetOccupancyManager assetOccupancyManager) : base(repository)
    {
        _repository = repository;
        _assetRepository = assetRepository;
        _assetCategoryRepository = assetCategoryRepository;
        _assetOccupancyManager = assetOccupancyManager;
    }

    protected override async Task<IQueryable<AssetOccupancy>> CreateFilteredQueryAsync(
        GetAssetOccupanciesRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        return query.WhereIf(input.AssetId.HasValue,
                x => x.AssetId == input.AssetId.Value)
            .WhereIf(input.Date.HasValue,
                x => x.Date == input.Date.Value);
    }

    [RemoteService(false)]
    public override Task<AssetOccupancyDto> UpdateAsync(Guid id, CreateAssetOccupancyDto input)
    {
        throw new NotSupportedException();
    }

    public override async Task<AssetOccupancyDto> CreateAsync(CreateAssetOccupancyDto input)
    {
        await CheckCreatePolicyAsync();

        var entity = await _assetOccupancyManager.CreateAsync(
            new OccupyAssetInfoModel(
                input.AssetId,
                input.Date,
                input.StartingTime,
                input.Duration),
            input.OccupierUserId);  // Todo: create a permission for occupying assets with a specified OccupierUserId.

        return await MapToGetOutputDtoAsync(entity);
    }

    public virtual async Task<AssetOccupancyDto> CreateByCategoryIdAsync(CreateAssetOccupancyByCategoryIdDto input)
    {
        await CheckCreatePolicyAsync();

        var entity = await _assetOccupancyManager.CreateByCategoryIdAsync(
            new OccupyAssetByCategoryInfoModel(
                input.AssetCategoryId,
                input.Date,
                input.StartingTime,
                input.Duration),
            input.OccupierUserId);  // Todo: create a permission for occupying assets with a specified OccupierUserId.

        return await MapToGetOutputDtoAsync(entity);
    }

    protected virtual async Task CheckSearchPolicyAsync()
    {
        await CheckPolicyAsync(SearchPolicyName);
    }
    
    protected virtual async Task CheckCreationCheckPolicyAsync()
    {
        await CheckPolicyAsync(CheckPolicyName);
    }

    public virtual async Task<List<BookablePeriodDto>> SearchAssetBookablePeriodsAsync(
        SearchAssetBookablePeriodsRequestDto input)
    {
        await CheckSearchPolicyAsync();

        var asset = await _assetRepository.GetAsync(input.AssetId);
        if (asset.Disabled)
        {
            return new List<BookablePeriodDto>();
        }

        var category = await _assetCategoryRepository.GetAsync(asset.AssetCategoryId);
        if (category.Disabled)
        {
            return new List<BookablePeriodDto>();
        }

        var periods = await _assetOccupancyManager.SearchAssetBookablePeriodsAsync(
            asset, category, input.CurrentTime, input.TargetDate);

        return ObjectMapper.Map<List<PeriodOccupancyModel>, List<BookablePeriodDto>>(periods);
    }

    public virtual async Task<List<BookablePeriodDto>> SearchCategoryBookablePeriodsAsync(
        SearchCategoryBookablePeriodsRequestDto input)
    {
        await CheckSearchPolicyAsync();
        var periods = await _assetOccupancyManager.SearchCategoryBookablePeriodsAsync(
            input.CategoryId, input.CurrentTime, input.TargetDate);

        return ObjectMapper.Map<List<Period>, List<BookablePeriodDto>>(periods);
    }

    public virtual async Task CheckCreateAsync(CreateAssetOccupancyDto input)
    {
        await CheckCreationCheckPolicyAsync();

        await _assetOccupancyManager.CreateAsync(
            new OccupyAssetInfoModel(
                input.AssetId,
                input.Date,
                input.StartingTime,
                input.Duration),
            input.OccupierUserId);  // Todo: create a permission for occupying assets with a specified OccupierUserId.

        await UnitOfWorkManager.Current.RollbackAsync();
    }

    public virtual async Task CheckCreateByCategoryIdAsync(
        CreateAssetOccupancyByCategoryIdDto input)
    {
        await CheckCreationCheckPolicyAsync();

        await _assetOccupancyManager.CreateByCategoryIdAsync(
            new OccupyAssetByCategoryInfoModel(
                input.AssetCategoryId,
                input.Date,
                input.StartingTime,
                input.Duration),
            input.OccupierUserId);  // Todo: create a permission for occupying assets with a specified OccupierUserId.

        await UnitOfWorkManager.Current.RollbackAsync();
    }

    public virtual async Task CheckBulkCreateAsync(BulkCreateAssetOccupancyDto input)
    {
        await CheckCreationCheckPolicyAsync();

        await _assetOccupancyManager.BulkCreateAsync(
            input.Models,
            input.ByCategoryModels,
            input.OccupierUserId);  // Todo: create a permission for occupying assets with a specified OccupierUserId.

        await UnitOfWorkManager.Current.RollbackAsync();
    }
}