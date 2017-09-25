using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger, IHostingEnvironment hostingEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                var path = context.Request.Path;
                var query = context.Request.Query;

                _logger.Error(exception, "ExceptionHandlerMiddleware: Exception caught at {path} - {query}", path, query);

                // TODO: review and specify other status codes if necessary
                string result = string.Empty;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                if (!_hostingEnvironment.IsProduction())
                {
                    result = JsonConvert.SerializeObject(new { message = exception.Message, stackTrace = exception.StackTrace });
                }

                return context.Response.WriteAsync(result);
            }
            catch (Exception e)
            {
                _logger.Error(e, "ExceptionHandlerMiddleware: nn error occured trying to handle an exception");
            }

            return Task.CompletedTask;
        }
    }
}
