using System;
using System.Threading.Tasks;
using Jarvis.Store.Client.Grains;
using Microsoft.Extensions.Logging;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Runtime.Services;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Jarvis.Store.Kernel.Grains
{
    public class SiloHouseKeeprServiceClient : GrainServiceClient<ISiloHouseKeeperService>, ISiloHouseKeeperServiceClient
    {
        public SiloHouseKeeprServiceClient(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task Clear(GrainReference grain)
        {
            return this.GrainService.Clear(grain);
        }
    }

    public class SiloHouseKeeperServiceGrain : GrainService, ISiloHouseKeeperService
    {
        private readonly ILogger _logger = Log.ForContext<SiloHouseKeeperServiceGrain>();

        public SiloHouseKeeperServiceGrain(IGrainIdentity id, Silo silo, ILoggerFactory loggerFactory): base(id, silo, loggerFactory)
        {
            
        }
        
        public Task Clear(GrainReference grain)
        {
            _logger.Information("Clear of {id} scheduled",grain.GrainIdentity.PrimaryKeyString);
            return Task.CompletedTask;
        }
    }
}