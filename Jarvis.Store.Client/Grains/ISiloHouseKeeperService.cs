using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Services;

namespace Jarvis.Store.Client.Grains
{
    public interface ISiloHouseKeeperService : IGrainService
    {
        Task Clear(GrainReference grain);
    }
}