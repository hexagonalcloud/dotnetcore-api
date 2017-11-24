using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace RestApi.Conventions
{
    public class ApiExplorerGroupPerNamespaceConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var groupName = controllerNamespace.Split('.').Last().ToLower();

            controller.ApiExplorer.GroupName = groupName;
        }
    }
}
