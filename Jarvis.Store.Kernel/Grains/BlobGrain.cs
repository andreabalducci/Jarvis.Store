using System.Threading.Tasks;
using Jarvis.Store.Client.Grains;
using Jarvis.Store.Kernel.Support;
using Orleans.Runtime;

namespace Jarvis.Store.Kernel.Grains
{
    public class BlobState
    {
        public int Reads { get; set; }
    }

    public class BlobGrain : Orleans.Grain, IBlobGrain
    {
        private readonly IPersistentState<BlobState> _state;
        private readonly ITenantContext _tenant;

        public BlobGrain([PersistentState("blob", "store-data")]IPersistentState<BlobState> state, ITenantContext tenant)
        {
            _state = state;
            _tenant = tenant;
        }

        public async Task<string> ReadAsync()
        {
            _state.State.Reads++;
            await _state.WriteStateAsync();
            return $" {_tenant.Name} => {_state.State.Reads}";
        }
    }
}