using Central_Service.Interface;
using Central_Service.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Repository_DAL_;
using Microsoft.Extensions.Configuration;

namespace Central_Service
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices( this IServiceCollection services,IConfiguration configuration )
        {
            services.AddDbContext<EFContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
