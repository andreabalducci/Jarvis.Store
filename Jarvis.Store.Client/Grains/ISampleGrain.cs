using System.Threading.Tasks;
using Orleans;

namespace Jarvis.Store.Client.Grains
{
    public interface ISampleGrain : IGrainWithStringKey
    {
        Task<string> ReadAsync();
    }
}