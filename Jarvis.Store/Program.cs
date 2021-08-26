using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jarvis.Store;
using Jarvis.Store.Kernel.Grains;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers.MongoDB.Configuration;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(sp => { })
    .UseOrleans(builder =>
    {
        builder
            .UseMongoDBClient("mongodb://127.0.0.1")
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
                })
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "jarvis-blob-store";
            })
            .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
            .ConfigureApplicationParts(parts => parts
                .AddApplicationPart(typeof(BlobGrain).Assembly)
                .WithReferences()
            )
            .UseDashboard(options => { options.Port = 8000; });
    })
    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
    .RunConsoleAsync();