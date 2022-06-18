using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.AssetOccupancyProviders;
using EasyAbp.BookingService.Assets;
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
    private readonly IAssetOccupancyProvider _assetOccupancyProvider;

    public AssetOccupancyAppService(IAssetOccupancyRepository repository,
        IAssetRepository assetRepository,
        IAssetCategoryRepository assetCategoryRepository,
        IAssetOccupancyProvider assetOccupancyProvider) : base(repository)
    {
        _repository = repository;
        _assetRepository = assetRepository;
        _assetCategoryRepository = assetCategoryRepository;
        _assetOccupancyProvider = assetOccupancyProvider;
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

        var (_, entity) = await _assetOccupancyProvider.OccupyAsync(
            new OccupyAssetInfoModel(
                input.AssetId,
                input.Volume,
                input.Date,
                input.StartingTime,
                input.Duration),
            input.OccupierUserId); // Todo: create a permission for occupying assets with a specified OccupierUserId.

        return await MapToGetOutputDtoAsync(entity);
    }

    public virtual async Task<AssetOccupancyDto> CreateByCategoryIdAsync(CreateAssetOccupancyByCategoryIdDto input)
    {
        await CheckCreatePolicyAsync();

        var (_, entity) = await _assetOccupancyProvider.OccupyByCategoryAsync(
            new OccupyAssetByCategoryInfoModel(
                input.AssetCategoryId,
                input.Volume,
                input.Date,
                input.StartingTime,
                input.Duration),
            input.OccupierUserId); // Todo: create a permission for occupying assets with a specified OccupierUserId.

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

    public virtual async Task<SearchBookingPeriodsResultDto> SearchBookingPeriodsAsync(
        SearchBookingPeriodsInputDto input)
    {
        await CheckSearchPolicyAsync();

        var asset = await _assetRepository.GetAsync(input.AssetId);
        var category = await _assetCategoryRepository.GetAsync(asset.AssetCategoryId);

        var periods = await _assetOccupancyProvider.GetPeriodsAsync(
            asset, category, input.TargetDate, input.CurrentDateTime);

        return new SearchBookingPeriodsResultDto(
            ObjectMapper.Map<List<PeriodOccupancyModel>, List<BookingPeriodDto>>(periods));
    }

    public virtual async Task<SearchBookingPeriodsResultDto> SearchCategoryBookingPeriodsAsync(
        SearchCategoryBookingPeriodsInputDto input)
    {
        await CheckSearchPolicyAsync();

        var category = await _assetCategoryRepository.GetAsync(input.CategoryId);

        var periods = await _assetOccupancyProvider.GetPeriodsAsync(
            category, input.TargetDate, input.CurrentDateTime);

        return new SearchBookingPeriodsResultDto(
            ObjectMapper.Map<List<PeriodOccupancyModel>, List<BookingPeriodDto>>(periods));
    }

    public virtual async Task CheckCreateAsync(CreateAssetOccupancyDto input)
    {
        await CheckCreationCheckPolicyAsync();

        await _assetOccupancyProvider.CanOccupyAsync(new OccupyAssetInfoModel(
            input.AssetId,
            input.Volume,
            input.Date,
            input.StartingTime,
            input.Duration)); // Todo: create a permission for occupying assets with a specified OccupierUserId.
    }

    public virtual async Task CheckCreateByCategoryIdAsync(
        CreateAssetOccupancyByCategoryIdDto input)
    {
        await CheckCreationCheckPolicyAsync();

        // Todo: create a permission for occupying assets with a specified OccupierUserId.
        await _assetOccupancyProvider.CanOccupyByCategoryAsync(new OccupyAssetByCategoryInfoModel(
            input.AssetCategoryId,
            input.Volume,
            input.Date,
            input.StartingTime,
            input.Duration));
    }

    public virtual async Task CheckBulkCreateAsync(BulkCreateAssetOccupancyDto input)
    {
        await CheckCreationCheckPolicyAsync();

        await _assetOccupancyProvider.CanBulkOccupyAsync(
            input.Models,
            input.ByCategoryModels); // Todo: create a permission for occupying assets with a specified OccupierUserId.
    }
}