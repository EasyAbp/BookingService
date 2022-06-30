using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetDefinitions;
using EasyAbp.BookingService.AssetOccupancyProviders;
using EasyAbp.BookingService.Assets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancies
{
    public class AssetOccupancyEventsHandlerTests : BookingServiceDomainTestBase
    {
        private readonly AssetOccupancyEventsHandler _assetOccupancyEventsHandler;
        private IAssetOccupancyProvider _assetOccupancyProvider;
        private IDistributedEventBus _distributedEventBus;
        private readonly IGuidGenerator _guidGenerator;

        public AssetOccupancyEventsHandlerTests()
        {
            _assetOccupancyEventsHandler = GetRequiredService<AssetOccupancyEventsHandler>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            base.AfterAddApplication(services);
            _assetOccupancyProvider = Substitute.For<IAssetOccupancyProvider>();
            _distributedEventBus = Substitute.For<IDistributedEventBus>();
            services.Replace(ServiceDescriptor.Transient(s => _assetOccupancyProvider));
            services.Replace(ServiceDescriptor.Transient(s => _distributedEventBus));
        }

        [Fact]
        public async Task OccupyAssetEvent_Baseline_Test()
        {
            // Arrange
            var requestId = _guidGenerator.Create();
            var userId = _guidGenerator.Create();
            var assetId = _guidGenerator.Create();
            const int volume = 1;
            var date = new DateTime(2022, 6, 18);
            var startingTime = TimeSpan.Zero;
            var duration = TimeSpan.FromHours(1);
            var model = new OccupyAssetInfoModel(assetId,
                volume,
                date,
                startingTime,
                duration);
            var eto = new OccupyAssetEto(default,
                requestId,
                userId,
                model);

            const string extraPropertyName = nameof(extraPropertyName);
            const string extraPropertyValue = nameof(extraPropertyValue);

            eto.SetProperty(extraPropertyName, extraPropertyValue, false);

            var assetOccupancy = new AssetOccupancy(_guidGenerator.Create(),
                default,
                assetId,
                nameof(Asset),
                nameof(AssetDefinition),
                volume,
                date,
                startingTime,
                duration,
                userId,
                nameof(userId));

            _assetOccupancyProvider.OccupyAsync(Arg.Any<OccupyAssetInfoModel>(), Arg.Any<Guid?>())
                .ReturnsForAnyArgs(
                    Task.FromResult<(ProviderAssetOccupancyModel, AssetOccupancy)>((null, assetOccupancy)));

            // Act
            await _assetOccupancyEventsHandler.HandleEventAsync(eto);

            // Assert
            await _distributedEventBus.Received().PublishAsync(
                Arg.Is<AssetOccupancyResultEto>(t =>
                    t.Success && t.TenantId == default && t.RequestId == requestId
                    && t.Model.Asset == assetOccupancy.Asset
                    && t.Model.Date == date
                    && t.Model.Duration == duration
                    && t.Model.Volume == volume
                    && t.Model.AssetId == assetId
                    && t.Model.OccupierName == assetOccupancy.OccupierName
                    && t.Model.StartingTime == startingTime
                    && t.Model.AssetDefinitionName == assetOccupancy.AssetDefinitionName
                    && t.Model.AssetOccupancyId == assetOccupancy.Id
                    && t.Model.OccupierUserId == userId
                    && t.GetProperty<string>(extraPropertyName, null) == extraPropertyValue
                ));
        }

        [Fact]
        public async Task OccupyAssetEvent_Failure_Test()
        {
            // Arrange
            var requestId = _guidGenerator.Create();
            var userId = _guidGenerator.Create();
            var assetId = _guidGenerator.Create();
            const int volume = 1;
            var date = new DateTime(2022, 6, 18);
            var startingTime = TimeSpan.Zero;
            var duration = TimeSpan.FromHours(1);
            var model = new OccupyAssetInfoModel(assetId,
                volume,
                date,
                startingTime,
                duration);
            var eto = new OccupyAssetEto(default,
                requestId,
                userId,
                model);

            _assetOccupancyProvider.OccupyAsync(Arg.Any<OccupyAssetInfoModel>(), Arg.Any<Guid?>())
                .ThrowsForAnyArgs(new BusinessException());

            // Act
            await Should.ThrowAsync<BusinessException>(() => _assetOccupancyEventsHandler.HandleEventAsync(eto));

            // Assert
            await _distributedEventBus.Received().PublishAsync(
                Arg.Is<AssetOccupancyResultEto>(t =>
                    !t.Success && t.TenantId == default && t.RequestId == requestId && t.Model == null
                ));
        }

        [Fact]
        public async Task OccupyAssetByCategoryEvent_Baseline_Test()
        {
            // Arrange
            var requestId = _guidGenerator.Create();
            var userId = _guidGenerator.Create();
            var assetId = _guidGenerator.Create();
            var categoryId = _guidGenerator.Create();
            const int volume = 1;
            var date = new DateTime(2022, 6, 18);
            var startingTime = TimeSpan.Zero;
            var duration = TimeSpan.FromHours(1);
            var model = new OccupyAssetByCategoryInfoModel(categoryId,
                volume,
                date,
                startingTime,
                duration);
            var eto = new OccupyAssetByCategoryEto(default,
                requestId,
                userId,
                model);

            const string extraPropertyName = nameof(extraPropertyName);
            const string extraPropertyValue = nameof(extraPropertyValue);

            eto.SetProperty(extraPropertyName, extraPropertyValue, false);

            var assetOccupancy = new AssetOccupancy(_guidGenerator.Create(),
                default,
                assetId,
                nameof(Asset),
                nameof(AssetDefinition),
                volume,
                date,
                startingTime,
                duration,
                userId,
                nameof(userId));

            _assetOccupancyProvider.OccupyByCategoryAsync(Arg.Any<OccupyAssetByCategoryInfoModel>(), Arg.Any<Guid?>())
                .ReturnsForAnyArgs(
                    Task.FromResult<(ProviderAssetOccupancyModel, AssetOccupancy)>((null, assetOccupancy)));

            // Act
            await _assetOccupancyEventsHandler.HandleEventAsync(eto);

            // Assert
            await _distributedEventBus.Received().PublishAsync(
                Arg.Is<AssetOccupancyResultEto>(t =>
                    t.Success && t.TenantId == default && t.RequestId == requestId
                    && t.Model.Asset == assetOccupancy.Asset
                    && t.Model.Date == date
                    && t.Model.Duration == duration
                    && t.Model.Volume == volume
                    && t.Model.AssetId == assetId
                    && t.Model.OccupierName == assetOccupancy.OccupierName
                    && t.Model.StartingTime == startingTime
                    && t.Model.AssetDefinitionName == assetOccupancy.AssetDefinitionName
                    && t.Model.AssetOccupancyId == assetOccupancy.Id
                    && t.Model.OccupierUserId == userId
                    && t.GetProperty<string>(extraPropertyName, null) == extraPropertyValue
                ));
        }

        [Fact]
        public async Task OccupyAssetByCategoryEvent_Failure_Test()
        {
            // Arrange
            var requestId = _guidGenerator.Create();
            var userId = _guidGenerator.Create();
            var assetId = _guidGenerator.Create();
            const int volume = 1;
            var date = new DateTime(2022, 6, 18);
            var startingTime = TimeSpan.Zero;
            var duration = TimeSpan.FromHours(1);
            var model = new OccupyAssetInfoModel(assetId,
                volume,
                date,
                startingTime,
                duration);
            var eto = new OccupyAssetEto(default,
                requestId,
                userId,
                model);

            _assetOccupancyProvider.OccupyAsync(Arg.Any<OccupyAssetInfoModel>(), Arg.Any<Guid?>())
                .ThrowsForAnyArgs(new BusinessException());

            // Act
            await Should.ThrowAsync<BusinessException>(() => _assetOccupancyEventsHandler.HandleEventAsync(eto));

            // Assert
            await _distributedEventBus.Received().PublishAsync(
                Arg.Is<AssetOccupancyResultEto>(t =>
                    !t.Success && t.TenantId == default && t.RequestId == requestId && t.Model == null
                ));
        }

        [Fact]
        public async Task BulkOccupyAssetEvent_Baseline_Test()
        {
            // Arrange
            var requestId = _guidGenerator.Create();
            var userId = _guidGenerator.Create();
            var assetId = _guidGenerator.Create();
            var categoryId = _guidGenerator.Create();
            const int volume = 1;
            var date = new DateTime(2022, 6, 18);
            var startingTime = TimeSpan.Zero;
            var duration = TimeSpan.FromHours(1);
            var categoryInfoModel = new OccupyAssetByCategoryInfoModel(categoryId,
                volume,
                date,
                startingTime,
                duration);
            var eto = new BulkOccupyAssetEto(default,
                requestId,
                userId,
                new List<OccupyAssetInfoModel>(),
                new List<OccupyAssetByCategoryInfoModel> { categoryInfoModel });

            const string extraPropertyName = nameof(extraPropertyName);
            const string extraPropertyValue = nameof(extraPropertyValue);

            eto.SetProperty(extraPropertyName, extraPropertyValue, false);

            var assetOccupancies = new List<AssetOccupancy>
            {
                new AssetOccupancy(_guidGenerator.Create(),
                    default,
                    assetId,
                    nameof(Asset),
                    nameof(AssetDefinition),
                    volume,
                    date,
                    startingTime,
                    duration,
                    userId,
                    nameof(userId))
            };

            _assetOccupancyProvider.BulkOccupyAsync(
                    Arg.Any<List<OccupyAssetInfoModel>>(),
                    Arg.Any<List<OccupyAssetByCategoryInfoModel>>(),
                    Arg.Any<Guid?>())
                .ReturnsForAnyArgs(Task.FromResult<List<(ProviderAssetOccupancyModel, AssetOccupancy)>>(
                    assetOccupancies.Select(x => (default(ProviderAssetOccupancyModel), x)).ToList()));

            // Act
            await _assetOccupancyEventsHandler.HandleEventAsync(eto);

            // Assert
            await _distributedEventBus.Received().PublishAsync(
                Arg.Is<BulkAssetOccupancyResultEto>(t =>
                    t.Success && t.TenantId == default && t.RequestId == requestId
                    && t.Models.Count == assetOccupancies.Count
                    && t.Models[0].Asset == assetOccupancies[0].Asset
                    && t.Models[0].Date == date
                    && t.Models[0].Duration == duration
                    && t.Models[0].Volume == volume
                    && t.Models[0].AssetId == assetId
                    && t.Models[0].OccupierName == assetOccupancies[0].OccupierName
                    && t.Models[0].StartingTime == startingTime
                    && t.Models[0].AssetDefinitionName == assetOccupancies[0].AssetDefinitionName
                    && t.Models[0].AssetOccupancyId == assetOccupancies[0].Id
                    && t.Models[0].OccupierUserId == userId
                    && t.GetProperty<string>(extraPropertyName, null) == extraPropertyValue
                ));
        }

        [Fact]
        public async Task BulkOccupyAssetEvent_Failure_Test()
        {
            // Arrange
            var requestId = _guidGenerator.Create();
            var userId = _guidGenerator.Create();
            var assetId = _guidGenerator.Create();
            var categoryId = _guidGenerator.Create();
            const int volume = 1;
            var date = new DateTime(2022, 6, 18);
            var startingTime = TimeSpan.Zero;
            var duration = TimeSpan.FromHours(1);
            var categoryInfoModel = new OccupyAssetByCategoryInfoModel(categoryId,
                volume,
                date,
                startingTime,
                duration);
            var eto = new BulkOccupyAssetEto(default,
                requestId,
                userId,
                new List<OccupyAssetInfoModel>(),
                new List<OccupyAssetByCategoryInfoModel> { categoryInfoModel });

            _assetOccupancyProvider.BulkOccupyAsync(
                    Arg.Any<List<OccupyAssetInfoModel>>(),
                    Arg.Any<List<OccupyAssetByCategoryInfoModel>>(),
                    Arg.Any<Guid?>())
                .ThrowsForAnyArgs(new BusinessException());

            // Act
            await Should.ThrowAsync<BusinessException>(() => _assetOccupancyEventsHandler.HandleEventAsync(eto));

            // Assert
            await _distributedEventBus.Received().PublishAsync(
                Arg.Is<AssetOccupancyResultEto>(t =>
                    !t.Success && t.TenantId == default && t.RequestId == requestId && t.Model == null
                ));
        }
    }
}