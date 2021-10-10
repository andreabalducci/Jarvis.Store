using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Orleans;

namespace Jarvis.Store.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(sp => { })
                .UseOrleans(builder =>
                {
                    builder
                        //.UseMongo()
                        .MemoryDevMode()
                        .ConfigureCluster()
                        .UseDashboard(options => { options.Port = 8000; });
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}