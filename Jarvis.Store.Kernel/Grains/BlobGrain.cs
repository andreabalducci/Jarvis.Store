using System.Threading.Tasks;
using Jarvis.Store.Client.Grains;

namespace Jarvis.Store.Kernel.Grains
{
    public class BlobGrain : Orleans.Grain, IBlobGrain
    {
        private int _version = 0;
        public Task<int> GetVersion()
        {
            _version++;
            return Task.FromResult(_version);
        }
    }
}