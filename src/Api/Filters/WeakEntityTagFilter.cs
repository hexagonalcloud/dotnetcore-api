using System;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class WeakEntityTagFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Method == "GET")
            {
                if (context.HttpContext.Response.StatusCode == 200)
                {
                    var result = context.Result as ObjectResult;
                    if (result == null)
                    {
                        return;
                    }

                    var weakEntityTag = result?.Value as IBaseEntity;
                    if (weakEntityTag != null)
                    {
                        var modifiedDate = weakEntityTag.ModifiedDate.ToUniversalTime();
                        var identifier = modifiedDate.ToBinary().ToString();
                        var eTag = "W/" + '"' + identifier + '"';

                        if (context.HttpContext.Request.Headers.Keys.Contains("If-None-Match") &&
                            context.HttpContext.Request.Headers["If-None-Match"].ToString() == eTag)
                        {
                            context.Result = new StatusCodeResult(304);
                        }

                        context.HttpContext.Response.Headers.Add("Cache-Control", "no-cache, must-revalidate");
                        context.HttpContext.Response.Headers.Add("ETag", new[] { eTag });
                        context.HttpContext.Response.Headers.Add("Last-Modified", modifiedDate.ToString("R"));
                    }
                }
            }
        }
    }
}
