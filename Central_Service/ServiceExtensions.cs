using Central_Service.Interface;
using Central_Service.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices( this IServiceCollection services )
        {
            services.AddScoped<ITestService, TestService>();
        }
    }
}
