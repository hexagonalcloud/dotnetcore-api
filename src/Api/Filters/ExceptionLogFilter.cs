using System.Dynamic;
using System.Threading.Tasks;
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
            var controllerName = string.Empty;
            var actionName = string.Empty;

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                controllerName = descriptor.ControllerName;
                actionName = descriptor.ActionName;
            }

            _logger.Error(context.Exception, "Error Action {ControllerName} - {ActionName}", controllerName, actionName);

            context.ExceptionHandled = true;

            if (_hostingEnvironment.IsDevelopment())
            {
                dynamic errorResult = new ExpandoObject();
                errorResult.Message = context.Exception.Message;
                errorResult.StackTrace = context.Exception.StackTrace;

                var result = new JsonResult(errorResult);
                result.StatusCode = 500;
                context.Result = result;
            }
            else
            {
                var result = new StatusCodeResult(500);
                context.Result = result;
            }

            return Task.CompletedTask;
        }
    }
}
