# dotnetcore-api

Prototype .NET Core API implementation

## Parts
- Authentication and authorizaton using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) (OpenID Connect and OAuth 2.0).
- [Autofac](https://autofac.org/) for dependency injection.
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) for REST API documentation.
- API Integration tests using [Autorest](https://github.com/Azure/AutoRest) (or [Swagger-codegen](https://github.com/swagger-api/swagger-codegen)?).
    - npm install -g autorest   
    - autorest --input-file=http://localhost:62520/swagger/v1/swagger.json --csharp --output-folder=AutoRestClient
    - To run the test locally for now (VS 2017 MacOS):
        - Start the API on the command line ("dotnet run" in the RestApi dir)
        - Run or debug the IntegrationTests
        - Using [Rider](https://www.jetbrains.com/rider/) we can run both the API and the integration tests.
        - TODO: 
            - Check the [integration test server](https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing) that is included in the framework.
            - How to handle evnironment variables (like the URL) for integration tests (if we do not use the integration server)?
- CI and CD to Azure App Services using VSTS
    - [API on Azure](http://dotnetcore-api.azurewebsites.net/api/values) 

## Azure
- [Api](https://dotnetcore-api.azurewebsites.net/)
- [Auth](https://dotnetcore-auth.azurewebsites.net/)

## A solution to define a local url that seems to work across all tools
- add "server.urls=http://localhost:5001" to the program args

## Integration tests
- For now not using the testserver, but just running the regular api and run the tests against that.
- For CI we do not want to run against a test server but against the deployed api.
- To use the correct config for dev (local), make sure the enviroment is set correctly to Development before running the tests. (https://dotnetcoretutorials.com/2017/05/03/environments-asp-net-core/)





    



    