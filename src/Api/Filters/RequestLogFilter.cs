using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Api.Filters
{
	public class RequestLogFilter: IAsyncActionFilter
	{
		private readonly ILogger _logger;
			
		public RequestLogFilter(ILogger logger)
		{
			this._logger = logger;
		}
		
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var controllerName = string.Empty;
			var actionName = string.Empty;
			
			var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
			if (descriptor != null)
			{
				controllerName = descriptor.ControllerName;
				actionName = descriptor.ActionName;
			}			
			
			var args = context.ActionArguments;

			// TODO: what else do we need here: application name, machine name? 

			 _logger.Information("Executing Action {ControllerName} - {ActionName} - {@Args} ", controllerName,
				actionName, args);
	
			var actionExecuted = await next(); // the actual action

			var result = actionExecuted.Result;	

			_logger.Information("Executed Action {ControllerName} - {ActionName} - {@Result}", controllerName, actionName, result);		
		}
	}
}
