using Central_Service.Interface;
using Central_Service.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Repository_DAL_;
using Microsoft.Extensions.Configuration;
using Central_Service.JWT;

namespace Central_Service
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices( this IServiceCollection services,IConfiguration configuration )
        {
            services.AddDbContext<EFContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Transient);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPaymentService, PaymentService>(); 
            services.AddScoped<IAppDataService, AppDataService>();
            services.AddScoped<IReservationService, ReservationService>();


            services.AddScoped<TokenHelper>();

            services.AddTransient<IObjectFactory, Service.ObjectFactory>();



        }
    }
}
