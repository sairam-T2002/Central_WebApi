using Central_Service.JWT;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Central_WebApi.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly TokenHelper _token;

        public TokenValidationMiddleware( RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            _token = new TokenHelper();
        }

        public async Task InvokeAsync( HttpContext context )
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor actionDescriptor)
            {
                if (actionDescriptor.ControllerName == "Auth" && actionDescriptor.ActionName != "Refresh")
                {
                    await _next(context);
                }
                else
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    if (token == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                    if (_token.IsTokenExpired(token))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                    try
                    {
                        await _next(context);
                    }
                    catch
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }
                }
            }
        }
    }
}
