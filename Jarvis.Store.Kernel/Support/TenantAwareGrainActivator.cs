using System;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace Jarvis.Store.Kernel.Support
{
    public class TenantAwareGrainActivator : DefaultGrainActivator
    {
        private readonly ILogger<TenantAwareGrainActivator> _logger;
        public TenantAwareGrainActivator(IServiceProvider serviceProvider, ILogger<TenantAwareGrainActivator> logger) : base(serviceProvider)
        {
            _logger = logger;
        }

        public override object Create(IGrainActivationContext context)
        {
            _logger.LogInformation($"Creating {context.GrainType.Name} {context.GrainIdentity.PrimaryKeyString}");
            return base.Create(context);
        }
    }
}