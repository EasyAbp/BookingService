using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Guids;
using Xunit;

namespace EasyAbp.BookingService.AssetSchedules
{
    public class AssetScheduleManagerTests : BookingServiceDomainTestBase
    {
        private readonly IAssetScheduleRepository _assetScheduleRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly AssetScheduleManager _assetScheduleManager;

        public AssetScheduleManagerTests()
        {
            _assetScheduleRepository = GetRequiredService<IAssetScheduleRepository>();
            _assetScheduleManager = GetRequiredService<AssetScheduleManager>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
        }

        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var date = DateTime.Now.Date;
            var assetId = _guidGenerator.Create();
            var periodSchemeId = _guidGenerator.Create();
            var periodId = _guidGenerator.Create();
            var usable = PeriodUsable.Accept;

            // Assert
            var assetSchedule =
                await _assetScheduleManager.CreateAsync(date, assetId, periodSchemeId, periodId, usable, null);

            // Assert
            assetSchedule.Date.ShouldBe(date);
            assetSchedule.AssetId.ShouldBe(assetId);
            assetSchedule.PeriodSchemeId.ShouldBe(periodSchemeId);
            assetSchedule.PeriodId.ShouldBe(periodId);
            assetSchedule.PeriodUsable.ShouldBe(usable);
            assetSchedule.TimeInAdvance.ShouldBeNull();
        }

        [Fact]
        public async Task Create_ShouldThrow_AssetScheduleExistsException_Test()
        {
            // Arrange
            var date = DateTime.Now.Date;
            var assetId = _guidGenerator.Create();
            var periodSchemeId = _guidGenerator.Create();
            var periodId = _guidGenerator.Create();
            var usable = PeriodUsable.Accept;
            var assetSchedule =
                await _assetScheduleManager.CreateAsync(date, assetId, periodSchemeId, periodId, usable, null);
            await WithUnitOfWorkAsync(() => _assetScheduleRepository.InsertAsync(assetSchedule));

            // Act & Assert
            await Should.ThrowAsync<AssetScheduleExistsException>(() =>
                _assetScheduleManager.CreateAsync(date, assetId, periodSchemeId, periodId,
                    PeriodUsable.Reject, null));
        }
    }
}