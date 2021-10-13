using System;
using Jarvis.Store.Kernel.Grains;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers.MongoDB.Configuration;

namespace Jarvis.Store.Service
{
    public static class SiloExtensions
    {
        public static ISiloBuilder MemoryDevMode(this ISiloBuilder siloBuilder)
        {
            return siloBuilder.UseLocalhostClustering()
                .AddMemoryGrainStorage("store-data");
        }

        public static ISiloBuilder ConfigureCluster(this ISiloBuilder siloBuilder, int siloPort, int gatewayPort)
        {
            return siloBuilder.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "jarvis-blob-store";
                })
                .Configure<ClusterMembershipOptions>(options =>
                {
//                    options.DefunctSiloCleanupPeriod = TimeSpan.FromSeconds(60);
//                    options.DefunctSiloExpiration = TimeSpan.FromSeconds(30);
                    //options.TableRefreshTimeout = TimeSpan.FromSeconds(100);
                    
                    // default 3
//                    options.NumMissedProbesLimit = 1;

                    // default 2
//                    options.NumVotesForDeathDeclaration = 1;
                })
                .ConfigureEndpoints(siloPort, gatewayPort)
                .ConfigureApplicationParts(parts => parts
                    .AddApplicationPart(typeof(BlobGrain).Assembly)
                    .WithReferences()
                );
        }

        public static ISiloBuilder UseMongo(this ISiloBuilder siloBuilder)
        {
            return siloBuilder.UseMongoDBClient("mongodb://127.0.0.1")
                .UseMongoDBClustering(options =>
                {
                    options.Strategy = MongoDBMembershipStrategy.SingleDocument;
                    options.DatabaseName = "jarvis-blobstore-cluster";
                })
                .AddMongoDBGrainStorage("store-data",
                    options =>
                    {
                        options.DatabaseName = "jarvis-blobstore-data";
                        options.ConfigureJsonSerializerSettings = settings =>
                        {
                            settings.NullValueHandling = NullValueHandling.Include;
                            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                            settings.DefaultValueHandling = DefaultValueHandling.Populate;
                        };
                    });
        }
    }
}