using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Api.Filters
{
	public class RequestLogFilter: IActionFilter
	{
		private readonly ILogger _logger;
		
		private string _controllerName = string.Empty;
		private string _actionName = string.Empty;
			
		public RequestLogFilter(ILogger logger)
		{
			this._logger = logger;
		}
		
		
		public void OnActionExecuting(ActionExecutingContext context)
		{
			
			var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
			if (descriptor != null)
			{
				_actionName = descriptor.ActionName;
				_controllerName = descriptor.ControllerName;

			}			
			var args = context.ActionArguments;

			// TODO: what else do we need here: application name, machine name? 
			
			_logger.Information("Executing Action {ControllerName} - {ActionName} - {@Args} ", _controllerName, _actionName, args)	
			;
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			var args = context.Result;	
			_logger.Information("Executed Action {ControllerName} - {ActionName} - {@Args} ", _controllerName, _actionName, args);
		}
	}
}
