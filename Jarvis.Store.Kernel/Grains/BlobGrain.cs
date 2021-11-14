using System.Threading.Tasks;
using Jarvis.Store.Client.Grains;
using Jarvis.Store.Kernel.Support;
using Orleans.Runtime;

namespace Jarvis.Store.Kernel.Grains
{
    public class BlobGrain : Orleans.Grain, IBlobGrain
    {
        private readonly ITenantContext _tenant;
        private int _counter;
        public BlobGrain( ITenantContext tenant)
        {
            _tenant = tenant;
        }

        public Task<string> ReadAsync()
        {
            _counter++;
            return Task.FromResult($" {_tenant.Name} => {_counter}");
        }
    }
}