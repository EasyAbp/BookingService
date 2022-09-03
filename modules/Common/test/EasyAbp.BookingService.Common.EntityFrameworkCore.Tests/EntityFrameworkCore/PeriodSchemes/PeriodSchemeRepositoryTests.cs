using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.BookingService.EntityFrameworkCore.PeriodSchemes
{
    public class PeriodSchemeRepositoryTests : BookingServiceCommonEntityFrameworkCoreTestBase
    {
        private readonly IPeriodSchemeRepository _periodSchemeRepository;
        private readonly PeriodSchemeManager _periodSchemeManager;

        public PeriodSchemeRepositoryTests()
        {
            _periodSchemeRepository = GetRequiredService<IPeriodSchemeRepository>();
            _periodSchemeManager = GetRequiredService<PeriodSchemeManager>();
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task FindDefaultScheme_Baseline_Test(bool isFound)
        {
            var entity = await _periodSchemeManager.CreateAsync(nameof(PeriodScheme) + "1", new List<Period>());

            await WithUnitOfWorkAsync(async () =>
            {
                // Arrange
                var anotherEntity =
                    await _periodSchemeManager.CreateAsync(nameof(PeriodScheme) + "2", new List<Period>());
                if (isFound)
                {
                    await _periodSchemeManager.SetAsDefaultAsync(entity);
                }

                await _periodSchemeRepository.InsertManyAsync(new[] { entity, anotherEntity });
            });


            // Act
            var result = await WithUnitOfWorkAsync(() => _periodSchemeRepository.FindDefaultSchemeAsync());

            // Assert
            if (isFound)
            {
                result.ShouldNotBeNull();
                result.IsDefault.ShouldBeTrue();
                result.Name.ShouldBe(entity.Name);
            }
            else
            {
                result.ShouldBeNull();
            }
        }
    }
}