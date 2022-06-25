using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace EasyAbp.BookingService.PeriodSchemes
{
    public class PeriodSchemeAppServiceTests : BookingServiceApplicationTestBase
    {
        private readonly IPeriodSchemeAppService _periodSchemeAppService;
        private readonly IPeriodSchemeRepository _periodSchemeRepository;
        private PeriodSchemeManager _periodSchemeManager;

        public PeriodSchemeAppServiceTests()
        {
            _periodSchemeAppService = GetRequiredService<IPeriodSchemeAppService>();
            _periodSchemeRepository = GetRequiredService<IPeriodSchemeRepository>();
            _periodSchemeManager.LazyServiceProvider = GetRequiredService<IAbpLazyServiceProvider>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            base.AfterAddApplication(services);
            _periodSchemeManager = Substitute.ForPartsOf<PeriodSchemeManager>();
            services.Replace(ServiceDescriptor.Transient(_ => _periodSchemeManager));
        }

        [Fact]
        public async Task GetList_Baseline_Test()
        {
            // Arrange
            var names = new HashSet<string>
            {
                "Name1",
                "Name2",
            };

            var entities = await CreateEntitiesAsync(names);

            await WithUnitOfWorkAsync(() =>
                _periodSchemeRepository.InsertManyAsync(entities));

            foreach (var input in GetInputs(names))
            {
                // Act
                var result = await WithUnitOfWorkAsync(() => _periodSchemeAppService.GetListAsync(input));

                // Assert
                // ReSharper disable PossibleInvalidOperationException
                var expected = entities.WhereIf(!input.Name.IsNullOrWhiteSpace(),
                        x => x.Name == input.Name)
                    .ToList();
                // ReSharper restore PossibleInvalidOperationException

                result.Items.Count.ShouldBe(expected.Count);
                foreach (var dto in result.Items)
                {
                    var entity = expected.FirstOrDefault(x => x.Id == dto.Id);
                    entity.ShouldNotBeNull();
                }
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SetAsDefault_Baseline_Test(bool hasDefaultPeriodScheme)
        {
            // Arrange
            var existPeriodScheme = await _periodSchemeManager.CreateAsync(nameof(PeriodScheme), new List<Period>());
            if (hasDefaultPeriodScheme)
            {
                await _periodSchemeManager.SetAsDefaultAsync(existPeriodScheme);
            }

            var newPeriodScheme = await _periodSchemeManager.CreateAsync("name", new List<Period>());

            await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(existPeriodScheme));
            newPeriodScheme = await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(newPeriodScheme));

            // Act
            await _periodSchemeAppService.SetAsDefaultAsync(newPeriodScheme.Id);

            // Assert
            var defaultPeriodScheme = await WithUnitOfWorkAsync(() => _periodSchemeRepository.FindDefaultSchemeAsync());
            defaultPeriodScheme.ShouldNotBeNull();
            defaultPeriodScheme.EntityEquals(newPeriodScheme).ShouldBeTrue();
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]
        [InlineData(new int[] { 3, 2, 1 }, new int[] { 1, 2, 3 })]
        [InlineData(new int[] { 3, 2, 1 }, new int[] { 3, 2, 1 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 3, 2, 1 })]
        public async Task PeriodIsOrderedByStartingTime_Test(int[] startingTimeHours, int[] durationHours)
        {
            // Arrange
            var periods = new List<Period>();
            foreach (var startingTimeHour in startingTimeHours)
            {
                foreach (var durationHour in durationHours)
                {
                    periods.Add(await _periodSchemeManager.CreatePeriodAsync(TimeSpan.FromHours(startingTimeHour),
                        TimeSpan.FromHours(durationHour)));
                }
            }

            var periodScheme = await _periodSchemeManager.CreateAsync(nameof(PeriodScheme), periods);
            await WithUnitOfWorkAsync(() =>
                _periodSchemeRepository.InsertAsync(periodScheme));

            // Act
            var getResult = await _periodSchemeAppService.GetAsync(periodScheme.Id);
            var getListResult = await _periodSchemeAppService.GetListAsync(new GetPeriodSchemesRequestDto());

            // Assert
            var expected = periods.OrderBy(x => x.StartingTime).ThenBy(x => x.Duration).ToList();
            getResult.Periods.Count.ShouldBe(expected.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                expected[i].Id.ShouldBe(getResult.Periods[i].Id);
                expected[i].StartingTime.ShouldBe(getResult.Periods[i].StartingTime);
                expected[i].Duration.ShouldBe(getResult.Periods[i].Duration);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Delete_Test(bool shouldThrow)
        {
            // Arrange
            var periodScheme = await _periodSchemeManager.CreateAsync(nameof(PeriodScheme), new List<Period>());
            periodScheme = await WithUnitOfWorkAsync(() => _periodSchemeRepository.InsertAsync(periodScheme));

            _periodSchemeManager.WhenForAnyArgs(x => x.IsPeriodSchemeInUseAsync(Arg.Any<PeriodScheme>()))
                .DoNotCallBase();
            _periodSchemeManager.IsPeriodSchemeInUseAsync(Arg.Any<PeriodScheme>())
                .ReturnsForAnyArgs(Task.FromResult(shouldThrow));

            // Act
            if (shouldThrow)
            {
                await Should.ThrowAsync<CannotDeletePeriodSchemeInUseException>(
                    WithUnitOfWorkAsync(() => _periodSchemeAppService.DeleteAsync(periodScheme.Id)));
            }
            else
            {
                await Should.NotThrowAsync(
                    WithUnitOfWorkAsync(() => _periodSchemeAppService.DeleteAsync(periodScheme.Id)));
            }
        }

        private static IEnumerable<GetPeriodSchemesRequestDto> GetInputs(IEnumerable<string> names)
        {
            foreach (var name in new string[] { null }.Concat(names))
            {
                yield return new GetPeriodSchemesRequestDto
                {
                    Name = name,
                    MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                };
            }
        }

        private async Task<List<PeriodScheme>> CreateEntitiesAsync(HashSet<string> names)
        {
            var list = new List<PeriodScheme>();
            foreach (var name in names)
            {
                list.Add(await _periodSchemeManager.CreateAsync(name, new List<Period>()));
            }

            return list;
        }
    }
}