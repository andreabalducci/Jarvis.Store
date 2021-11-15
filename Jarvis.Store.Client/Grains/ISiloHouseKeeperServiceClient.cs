using System.Threading.Tasks;
using Orleans.CodeGeneration;
using Orleans.Runtime;
using Orleans.Services;

namespace Jarvis.Store.Client.Grains
{
    public interface ISiloHouseKeeperServiceClient : IGrainServiceClient<ISiloHouseKeeperService>
    {
        Task Clear(GrainReference grain);
    }
}