﻿using Central_Service;

namespace Central_WebApi
{
    public static class ServiceExtension
    {
        public static void AddWebApiCors( this IServiceCollection services, IConfiguration configuration, string policyName )
        {
            var settingsSection = configuration.GetSection("CorsIPs");
            var settings = settingsSection.Get<string[]>();
            settings = settings ?? new string[0];
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                    //builder.WithOrigins(settings)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        public static void AddApiServices( this IServiceCollection services )
        {
            services.ConfigureServices();
        }
    }
}
