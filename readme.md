# dotnetcore-api

Prototype .NET Core API implementation

## Parts

- Authentication and authorizaton using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) (OpenID Connect and OAuth 2.0).
- [Autofac](https://autofac.org/) for dependency injection.
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) for API documentation generation.
- API Integration tests using [Autorest](https://github.com/Azure/AutoRest).
- CI and CD to Azure App Services using VSTS

## Autorest
- npm install -g autorest   
- autorest --input-file=http://localhost:5001/swagger/v1/swagger.json --csharp --output-folder=AutoRestClient  

## Azure
- [Api](https://dotnetcore-api.azurewebsites.net/) and [Swagger UI](https://dotnetcore-api.azurewebsites.net/swagger/ui)
- [Auth](https://dotnetcore-auth.azurewebsites.net/)

## Integration tests
- For now not using the included [integration test server](https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing), but just running the regular api and run the tests against that.
- For CI we do not want to run against a test server but against the deployed api.
- To use the correct config for dev (local), make sure the enviroment is set correctly to Development before running the tests. (https://dotnetcoretutorials.com/2017/05/03/environments-asp-net-core/)
- To run the intregation tests locally on MacOS:
    - VS 2017: 
        - Start the API on the command line ("dotnet run" in the RestApi dir)
        - Run or debug the IntegrationTests
    - Using [Rider](https://www.jetbrains.com/rider/) we can run both the API and the integration tests.
    - VS Code: we can run and debug both simultaneoouly.

## A solution to define a local url that seems to work across all tools
- add "server.urls=http://localhost:5001" to the program args

## TODO
- Check [Swagger-codegen](https://github.com/swagger-api/swagger-codegen) if we continue to go the swagger route for generating clients and tests.
- Rename the integration test classes to just 'Products' and 'Values' since they are alredy in a Tests folder?

    



    