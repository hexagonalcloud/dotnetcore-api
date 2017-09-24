using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Filters
{
    public class ResponseOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerPolicies = context.ApiDescription.ControllerAttributes().OfType<AuthorizeAttribute>();
            var actionPolicies = context.ApiDescription.ActionAttributes().OfType<AuthorizeAttribute>();

            var policies = controllerPolicies.Union(actionPolicies).Distinct();

            if (policies.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();

                operation.Security.Add(
                    new Dictionary<string, IEnumerable<string>>
                    {
                            // add the scope to let it show up in the Swagger UI.
                            // If using claims this is where we can add them to show up in the UI
                            { "oauth2", new List<string>() { "api1" } }
                    });
            }

            operation.Responses.Add("429", new Response { Description = "Too Many Requests" });
        }
    }
}
