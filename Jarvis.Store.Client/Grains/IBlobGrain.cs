using System.Threading.Tasks;
using Orleans;

namespace Jarvis.Store.Client.Grains
{
    public interface IBlobGrain : IGrainWithStringKey
    {
        Task<string> ReadAsync();
    }
}