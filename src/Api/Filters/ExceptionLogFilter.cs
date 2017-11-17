using System;
using System.Dynamic;
using System.Threading.Tasks;
using Api.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Api.Filters
{
    public class ExceptionLogFilter : IAsyncExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public ExceptionLogFilter(IHostingEnvironment hostingEnvironment, ILogger logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var userInfo = new UserInfo(context.HttpContext.User); // TODO: or add this using an enricher? It should not be part of the message but should be in the context

            var errorId = Guid.NewGuid();

            var controllerName = string.Empty;
            var actionName = string.Empty;

            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                controllerName = descriptor.ControllerName;
                actionName = descriptor.ActionName;
            }

            _logger.Error(context.Exception, "Error Action {ControllerName} - {ActionName} - {ErrorId} - {@UserInfo}", controllerName, actionName, errorId, userInfo);

            context.ExceptionHandled = true;

            if (_hostingEnvironment.IsDevelopment())
            {
                dynamic errorResult = new ExpandoObject();
                errorResult.Message = context.Exception.Message;
                errorResult.StackTrace = context.Exception.StackTrace;
                errorResult.ErrorId = errorId;

                var result = new JsonResult(errorResult);
                result.StatusCode = 500;
                context.Result = result;
            }
            else
            {
                // TODO: return custom ErrorResult and configure Swagger responses
                dynamic errorResult = new ExpandoObject();
                errorResult.ErrorId = errorId;

                var result = new JsonResult(errorResult);
                result.StatusCode = 500;
                context.Result = result;
            }

            return Task.CompletedTask;
        }
    }
}
