using System.Threading.Tasks;
using Api.Logging;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Api.Filters
{
    public class RequestLogFilter : IAsyncActionFilter
    {
        private readonly ILogger _logger;

        public RequestLogFilter(ILogger logger)
        {
            this._logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userInfo = new UserInfo(context.HttpContext.User); // TODO: or add this using an enricher? It should not be part of the message but should be in the context

            var controllerName = string.Empty;
            var actionName = string.Empty;

            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                controllerName = descriptor.ControllerName;
                actionName = descriptor.ActionName;
            }

            var args = context.ActionArguments;

             _logger.Information("Executing Action {ControllerName} - {ActionName} - {@Args} - {@userInfo} ", controllerName, actionName, args, userInfo);

            var actionExecuted = await next(); // the actual action

            var result = actionExecuted.Result;

            _logger.Information("Executed Action {ControllerName} - {ActionName} - {@Result}", controllerName, actionName, result);
        }
    }
}
