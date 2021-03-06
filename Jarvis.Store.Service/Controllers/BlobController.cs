using System.Threading.Tasks;
using Jarvis.Store.Client.Grains;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace Jarvis.Store.Service.Controllers
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

        [HttpGet("read/{id}")]
        public async Task<string> Read(string id)
        {
            var grain = _client.GetGrain<ISampleGrain>(id);
            return await grain.ReadAsync();
        }
    }
}