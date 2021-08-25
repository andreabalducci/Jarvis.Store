using System.Threading.Tasks;
using Orleans;

namespace Jarvis.Store.Grains
{
    public interface IBlobGrain : IGrainWithStringKey
    {
        Task<int> GetVersion();
    }

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