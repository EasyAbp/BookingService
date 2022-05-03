using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.Assets;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Services;

namespace EasyAbp.BookingService.AssetOccupancies;

public class AssetOccupancyManager : DomainService, IAssetOccupancyManager
{
    private readonly IAssetOccupancyRepository _repository;
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetPeriodSchemeRepository _assetPeriodSchemeRepository;
    private readonly BookingServiceOptions _options;

    public AssetOccupancyManager(IAssetOccupancyRepository repository,
        IAssetRepository assetRepository,
        IAssetPeriodSchemeRepository assetPeriodSchemeRepository,
        IOptions<BookingServiceOptions> options)
    {
        _repository = repository;
        _assetRepository = assetRepository;
        _assetPeriodSchemeRepository = assetPeriodSchemeRepository;
        _options = options.Value;
    }

    public async Task<AssetOccupancy> CreateAsync(Guid assetId, DateTime date, TimeSpan startingTime, TimeSpan duration,
        Guid? occupierUserId)
    {
        var asset = await _assetRepository.GetAsync(assetId);
        var assetPeriodScheme =
            await _assetPeriodSchemeRepository.FindAsync(x => x.AssetId == assetId && x.Date == date);

        // TODO Need level concurrency lock here?

        // TODO CORE LOGIC, PENDING
        throw new NotImplementedException();
    }

    public Task UpdateAsync(AssetOccupancy entity, Guid assetId, DateTime date, TimeSpan startingTime,
        TimeSpan duration,
        Guid? occupierUserId)
    {
        // TODO Do we need Asset level concurrency lock?

        // TODO CORE LOGIC, PENDING
        throw new NotImplementedException();
    }
}