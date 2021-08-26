using System.Threading.Tasks;
using Jarvis.Store.Client.Grains;
using Jarvis.Store.Kernel.Grains;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace Jarvis.Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IClusterClient _client;

        public BlobController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet("version/{id}")]
        public async Task<int> GetVersion(string id)
        {
            var grain = _client.GetGrain<IBlobGrain>(id);
            return await grain.GetVersion();
        }
    }
}