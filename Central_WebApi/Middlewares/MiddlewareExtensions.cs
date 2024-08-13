namespace Central_WebApi.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseActionLogging( this IApplicationBuilder builder )
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }

        public static IApplicationBuilder UseTokenValidation( this IApplicationBuilder builder )
        {
            return builder.UseMiddleware<TokenValidationMiddleware>();
        }
    }
}
