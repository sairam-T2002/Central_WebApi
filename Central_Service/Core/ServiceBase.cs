using Central_Service.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Core
{
    public abstract class ServiceBase
    {
        protected readonly ILogger<ServiceBase> Logger;
        protected readonly IServiceProvider serviceProvider;
        protected ServiceBase( ILogger<ServiceBase> logger, IServiceProvider provider ) {
            Logger = logger;
            serviceProvider = provider;
        }

        protected T GetService<T>() where T : notnull
        {
            return serviceProvider.GetRequiredService<T>();
        }

        protected object GetService( Type serviceType )
        {
            object? service = serviceProvider.GetService(serviceType);
            if (service == null)
            {
                throw new InvalidOperationException("No service for type 'serviceType' has been registered.");
            }

            return service;
        }
    }
}
