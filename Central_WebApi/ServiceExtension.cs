using Central_Service;

namespace Central_WebApi
{
    public static class ServiceExtension
    {
        public static void AddWebApiCors( this IServiceCollection services, IConfiguration configuration, string policyName )
        {
            var settingsSection = configuration.GetSection("CorsIPs");
            string[]? settings = settingsSection?.Get<string[]>();
            settings = settings ?? Array.Empty < string>();

            services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        public static void AddApiServices( this IServiceCollection services,IConfiguration configuration )
        {
            services.ConfigureServices(configuration);
        }
    }
}
