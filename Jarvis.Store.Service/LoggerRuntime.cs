using Serilog;

namespace Jarvis.Store.Service
{
    public static class LoggerRuntime
    {
        public static void Init()
        {
            Log.Logger = new LoggerConfiguration()
              //  .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static void Shutdown()
        {
            Log.CloseAndFlush();
        }
    }
}