using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Guids;
using Xunit;

namespace EasyAbp.BookingService.AssetOccupancyProviders;

public class DefaultAssetOccupyTransactionLockTests : BookingServiceDomainTestBase
{
    [Fact]
    public async Task TryAcquire_Baseline_Test()
    {
        // arrange
        var guidGenerator = ServiceProvider.GetRequiredService<IGuidGenerator>();
        var defaultAssetOccupyTransactionLock = ServiceProvider.GetRequiredService<DefaultAssetOccupyTransactionLock>();
        var categoryId1 = guidGenerator.Create();
        var categoryId2 = guidGenerator.Create();
        var date1 = new DateTime(2022, 7, 22);
        var date2 = new DateTime(2022, 7, 22);
        var model1 = Substitute.For<IOccupyingTimeInfo>();
        var model2 = Substitute.For<IOccupyingTimeInfo>();
        model1.Date.ReturnsForAnyArgs(date1);
        model2.Date.ReturnsForAnyArgs(date2);

        var c1d1 = (categoryId1, model1);
        var c1d2 = (categoryId1, model2);
        var c2d1 = (categoryId2, model1);
        var c2d2 = (categoryId2, model2);

        var resources = new[] { c1d1, c1d2, c2d1, c2d2 };

        // act
        var handle = await defaultAssetOccupyTransactionLock.TryAcquireAsync(resources,
            TimeSpan.FromSeconds(5));

        // assert
        await Should.ThrowAsync<FailToObtainAssetOccupancyLockException>(() =>
            defaultAssetOccupyTransactionLock.TryAcquireAsync(new[] { c1d1, c1d2, c2d1, c2d2 },
                TimeSpan.FromSeconds(5)));
        foreach (var resource in resources)
        {
            await Should.ThrowAsync<FailToObtainAssetOccupancyLockException>(() =>
                defaultAssetOccupyTransactionLock.TryAcquireAsync(new[] { resource },
                    TimeSpan.FromSeconds(5)));
        }

        await handle.DisposeAsync();

        await using var h = await defaultAssetOccupyTransactionLock.TryAcquireAsync(new[] { c1d1, c1d2, c2d1, c2d2 },
            TimeSpan.FromSeconds(5));
    }


    [Fact]
    public async Task TryAcquire_ResourcesCompetition_Test()
    {
        // arrange
        var guidGenerator = ServiceProvider.GetRequiredService<IGuidGenerator>();
        var defaultAssetOccupyTransactionLock = ServiceProvider.GetRequiredService<DefaultAssetOccupyTransactionLock>();
        var categoryId1 = guidGenerator.Create();
        var categoryId2 = guidGenerator.Create();
        var date1 = new DateTime(2022, 7, 22);
        var date2 = new DateTime(2022, 7, 22);
        var model1 = Substitute.For<IOccupyingTimeInfo>();
        var model2 = Substitute.For<IOccupyingTimeInfo>();
        model1.Date.ReturnsForAnyArgs(date1);
        model2.Date.ReturnsForAnyArgs(date2);

        var c1d1 = (categoryId1, model1);
        var c1d2 = (categoryId1, model2);
        var c2d1 = (categoryId2, model1);
        var c2d2 = (categoryId2, model2);

        var resources1 = new[] { c1d1, c1d2, c2d1 };
        var resources2 = new[] { c1d1, c1d2, c2d2 };

        var manualResetEvent = new ManualResetEventSlim(false);
        // act
        var task1 = Task.Run(async () =>
        {
            try
            {
                await Task.Yield();
                manualResetEvent.Wait();
                return await defaultAssetOccupyTransactionLock.TryAcquireAsync(resources1,
                    TimeSpan.FromSeconds(5));
            }
            catch
            {
                return null;
            }
        });

        var task2 = Task.Run(async () =>
        {
            try
            {
                await Task.Yield();
                manualResetEvent.Wait();
                return await defaultAssetOccupyTransactionLock.TryAcquireAsync(resources2,
                    TimeSpan.FromSeconds(5));
            }
            catch
            {
                return null;
            }
        });

        await Task.Delay(100);
        manualResetEvent.Set();
        await Task.WhenAll(task1, task2);

        // assert
        (task1.Result is not null || task2.Result is not null).ShouldBeTrue();
        (task1.Result is not null && task2.Result is not null).ShouldBeFalse();
    }
}