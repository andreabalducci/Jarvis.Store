using System.Threading.Tasks;
using Jarvis.Store.Client.Grains;
using Jarvis.Store.Kernel.Support;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using Serilog;

namespace Jarvis.Store.Kernel.Grains
{
    public class SampleGrain : Orleans.Grain, ISampleGrain
    {
        private readonly ITenantContext _tenant;
        private readonly IGrainFactory _factory;
        private int _counter;
        private readonly ILogger _logger = Log.ForContext<SampleGrain>();
        private readonly ILocalMetricsGrain _metrics;
        private readonly ISiloHouseKeeperServiceClient _housekeeper;
        public SampleGrain(ITenantContext tenant, IGrainFactory factory, ISiloHouseKeeperServiceClient housekeeper)
        {
            _tenant = tenant;
            _factory = factory;
            _housekeeper = housekeeper;

            _metrics =  _factory.GetGrain<ILocalMetricsGrain>($"metrics@{tenant.Name}");
        }

        public async Task<string> ReadAsync()
        {
            _logger.Information("{id}, ReadAsync", this.GetGrainIdentity().PrimaryKeyString);
            _counter++;

            _metrics.Inc("ReadyAsync").Ignore();

            if (_counter % 3 == 0)
            {
                await _housekeeper.Clear(this.GrainReference);
            }
            
            return $" {_tenant.Name} => {_counter}";
        }
    }
}