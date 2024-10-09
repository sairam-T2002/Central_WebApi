using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Central_Service.Core
{
    public abstract class ServiceBase
    {
        protected readonly ILogger<ServiceBase> Logger;
        protected readonly IServiceProvider _serviceProvider;
        protected ServiceBase( ILogger<ServiceBase> logger, IServiceProvider provider ) {
            Logger = logger;
            _serviceProvider = provider;
        }

        protected T GetService<T>() where T : notnull
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        protected object GetService( Type serviceType )
        {
            object? service = _serviceProvider.GetService(serviceType);
            if (service == null)
            {
                throw new InvalidOperationException("No service for type 'serviceType' has been registered.");
            }

            return service;
        }

        protected IServiceScope CreateScope()
        {
            return _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }
    }
}
