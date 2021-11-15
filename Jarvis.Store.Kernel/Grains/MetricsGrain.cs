using System.Threading;
using System.Threading.Tasks;
using Jarvis.Store.Kernel.Support;
using Orleans;
using Orleans.Concurrency;
using Serilog;

namespace Jarvis.Store.Kernel.Grains
{
    public interface ILocalMetricsGrain : IGrainWithStringKey
    {
        Task Inc(string counter);
    }

    /// <summary>
    /// https://dotnet.github.io/orleans/docs/grains/stateless_worker_grains.html#reduce-style-aggregation
    /// </summary>
    [StatelessWorker]
    public class LocalMetricsGrain: Grain, ILocalMetricsGrain
    {
        private static int Counter = 0; 
        private readonly ILogger _logger = Log.ForContext<LocalMetricsGrain>();
        private readonly int _id = Interlocked.Increment(ref Counter);

        private readonly ITenantContext _tenantContext;

        public LocalMetricsGrain(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task Inc(string counter)
        {
            _logger.Information($"Metrics {_tenantContext.Name} {_id} - Inc");
            await Task.Delay(5000);
        }
    }
}