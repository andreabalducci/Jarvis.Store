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

        public static ISiloBuilder ConfigureCluster(this ISiloBuilder siloBuilder)
        {
            return siloBuilder.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "jarvis-blob-store";
                })
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .ConfigureApplicationParts(parts => parts
                    .AddApplicationPart(typeof(BlobGrain).Assembly)
                    .WithReferences()
                );
        }

        public  static ISiloBuilder UseMongo(this ISiloBuilder siloBuilder)
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