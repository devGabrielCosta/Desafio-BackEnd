using Amazon.Runtime.Internal;
using System.Net;
using System.Text.Json;

namespace Application.Configuration
{
    public static class ErrorHandlerExtensions
    {      
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<ErrorHandler>();
        }
    }
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _log;

        public ErrorHandler(RequestDelegate next, ILoggerFactory log)
        {
            this._next = next;
            this._log = log.CreateLogger("MyErrorHandler");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(httpContext, ex);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            var errorResponse = new ErrorResponse();

            errorResponse.StatusCode = HttpStatusCode.InternalServerError;
            errorResponse.Message = exception.Message;

            _log.LogError($"Error: {exception.Message}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)errorResponse.StatusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
