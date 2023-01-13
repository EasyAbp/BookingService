using System;
using EasyAbp.BookingService.AssetOccupancyProviders;
using Orleans.Hosting;
using Orleans.Serialization;
using Orleans.TestingHost;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.BookingService;

public class ClusterFixture : IDisposable, ITransientDependency
{
    public ClusterFixture()
    {
        var builder = new TestClusterBuilder();
        Cluster = builder
            .AddSiloBuilderConfigurator<TestSiloConfigurations>()
            .Build();
        Cluster.Deploy();
    }

    public void Dispose()
    {
        Cluster.StopAllSilos();
    }

    public TestCluster Cluster { get; }

    private class TestSiloConfigurations : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            // todo: upgrade to Orleans 7
            // siloBuilder.Services.AddSerializer(builder =>
            // {
            //     builder.AddJsonSerializer(isSupported: type =>
            //         type.Namespace?.StartsWith("EasyAbp.BookingService") ?? false);
            // });

            siloBuilder.AddMemoryGrainStorage(AssetOccupancyGrain.StorageProviderName);
        }
    }
}