using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace Jarvis.Store.Kernel.Support
{
    public interface ITenantContext
    {
        string Name { get; }
    }

    public class TenantContext : ITenantContext
    {
        public string Name { get; set; }
    }
    public class TenantAwareGrainActivator : DefaultGrainActivator
    {
        private readonly ILogger<TenantAwareGrainActivator> _logger;

        public TenantAwareGrainActivator(IServiceProvider serviceProvider, ILogger<TenantAwareGrainActivator> logger) :
            base(serviceProvider)
        {
            _logger = logger;
        }

        public override object Create(IGrainActivationContext context)
        {
            var id = context.GrainIdentity.PrimaryKeyString;
            if (!string.IsNullOrEmpty(id))
            {
                var tokens = id.Split('@');
                if (tokens.Length == 2)
                {
                    _logger.LogInformation(
                        $"Creating {context.GrainType.Name} {context.GrainIdentity.PrimaryKeyString}");
                    var tenant = (TenantContext) context.ActivationServices.GetRequiredService<ITenantContext>()  ;
                    tenant.Name = tokens[1];
                }
            }

            return base.Create(context);
        }
    }
}