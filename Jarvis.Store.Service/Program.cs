using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Invocation;
using Orleans;

namespace Jarvis.Store.Service
{
    public class Program
    {
        private static int SiloPort { get; set; } = 11111;
        private static int GatewayPort { get; set; } = 30000;
        private static int DashboardPort { get; set; } = 8000;
        private static bool WorkerNode { get; set; } = false;

        public static void Main(string[] args)
        {
            ParseCommandLine(args);
            CreateHostBuilder(args).Build().Run();
        }

        private static void ParseCommandLine(string[] args)
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

            rootCommand.Description = Constants.ApplicationName;

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<int, int, int, bool>((silo, gateway, dashboard, worker) =>
            {
                GatewayPort = gateway;
                SiloPort = silo;
                DashboardPort = dashboard;
                WorkerNode = worker;
            });

            // Parse the incoming args and invoke the handler
            rootCommand.Invoke(args);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices(sp => { })
                .UseOrleans(builder =>
                {
                    builder
                        .UseMongo()
                        //                        .MemoryDevMode()
                        .ConfigureCluster(SiloPort, GatewayPort);

                    if (DashboardPort > 0)
                    {
                        builder.UseDashboard(options => { options.Port = DashboardPort; });
                    }
                });

            if (!WorkerNode)
            {
                builder.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            }

            return builder;
        }
    }
}