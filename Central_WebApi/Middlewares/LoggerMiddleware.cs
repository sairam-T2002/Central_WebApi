using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text;

namespace Central_WebApi.Middlewares
{
    public static class ActionLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseActionLogging( this IApplicationBuilder builder )
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }
    }

    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerMiddleware> _logger;

        public LoggerMiddleware( RequestDelegate next, ILogger<LoggerMiddleware> logger )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync( HttpContext context )
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor actionDescriptor)
            {
                // Log action parameters from route
                var parameters = actionDescriptor.Parameters;
                foreach (var param in parameters)
                {
                    if (context.Request.RouteValues.TryGetValue(param.Name, out var value)) 
                    {
                        _logger.LogInformation($"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}/Action Parameter: {param.Name} = {value}");
                    }
                }

                // Log request body
                var requestBody = await ReadRequestBody(context.Request);
                _logger.LogInformation($"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}/Request Body: {requestBody}");

                // Capture the original body stream
                var originalBodyStream = context.Response.Body;
                try
                {
                    // Create a new memory stream to capture the response
                    using var responseBody = new MemoryStream();
                    context.Response.Body = responseBody;

                    // Call the next middleware
                    await _next(context);

                    // Log the response
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseContent = await new StreamReader(responseBody).ReadToEndAsync();
                    _logger.LogInformation($"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}/Action Response: {responseContent}");

                    // Copy the response to the original stream
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
                finally
                {
                    // Restore the original stream
                    context.Response.Body = originalBodyStream;
                }
            }
            else
            {
                // If it's not a controller action, just call the next middleware
                await _next(context);
            }
        }

        private async Task<string> ReadRequestBody( HttpRequest request )
        {
            request.EnableBuffering();

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return requestBody;
        }
    }
}