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
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(sp =>{})
    .UseOrleans(builder =>
    {
        builder.UseLocalhostClustering()
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "OrleansBasics";
            })
            .ConfigureApplicationParts(parts => parts
                .AddApplicationPart(typeof(BlobGrain).Assembly)
                .WithReferences())
            .UseDashboard(options => { options.Port = 8000; });
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .RunConsoleAsync();
