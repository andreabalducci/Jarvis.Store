using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Jarvis.Store.Kernel.Support;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Runtime;

namespace Jarvis.Store.Service
{
    public class Program
    {
        private static int SiloPort { get; set; } = 11111;
        private static int GatewayPort { get; set; } = 30000;
        private static int DashboardPort { get; set; } = 8000;
        private static bool WorkerNode { get; set; } = false;

        public static async Task<int> Main(string[] args)
        {
            var rootCommand = CreateCommandLineParser(args);

            // Parse the incoming args and invoke the handler
            return await rootCommand.InvokeAsync(args);
        }

        private static RootCommand CreateCommandLineParser(string[] args)
        {
            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                new Option<int>(
                    "--silo-port",
                    getDefaultValue: () => 11111,
                    description: "Orleans silo port"),
                new Option<int>(
                    "--gateway-port",
                    getDefaultValue: () => 30000,
                    "Orleans gateway port"),
                new Option<int>(
                    "--dashboard-port",
                    getDefaultValue: () => 0,
                    "Orleans dashboard port"),
                new Option<bool>(
                    "--worker",
                    getDefaultValue: () => false,
                    "Should run as worker node (no api / ui)")
            };

            rootCommand.Description = "Jarvis Store Service";

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<int, int, int, bool>(async (silo, gateway, dashboard, worker) =>
            {
                GatewayPort = gateway;
                SiloPort = silo;
                DashboardPort = dashboard;
                WorkerNode = worker;

                await CreateHostBuilder(args).Build().RunAsync();
            });
            return rootCommand;
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                    .UseOrleans(builder =>
                    {
                        builder
                            // .UseMongo()
                            .MemoryDevMode()
                            .ConfigureCluster(SiloPort, GatewayPort);

                        if (DashboardPort > 0)
                        {
                            builder.UseDashboard(options => { options.Port = DashboardPort; });
                        }
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton<IGrainActivator, TenantAwareGrainActivator>();
                        services.AddScoped<ITenantContext, TenantContext>();
                    })
                ;

            if (!WorkerNode)
            {
                builder.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            }

            return builder;
        }
    }
}