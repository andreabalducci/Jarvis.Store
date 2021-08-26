using System.Threading.Tasks;
using Jarvis.Store.Client.Grains;
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
        public BlobGrain([PersistentState("blob", "store-data")]IPersistentState<BlobState> state)
        {
            _state = state;
        }

        public async Task<int> ReadAsync()
        {
            _state.State.Reads++;
            await _state.WriteStateAsync();
            return _state.State.Reads;
        }
    }
}