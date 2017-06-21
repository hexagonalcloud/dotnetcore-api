# dotnetcore-api

Prototype .NET Core API implementation

- [Autofac](https://autofac.org/) for dependency injection.
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) for REST API documentation.
- API Integration tests using [Autorest](https://github.com/Azure/AutoRest) (or [Swagger-codegen](https://github.com/swagger-api/swagger-codegen)?).
    - npm install -g autorest   
    - autorest --input-file=http://localhost:62520/swagger/v1/swagger.json --csharp --output-folder=AutoRestClient
    - To run the test locally for now:
        - Start the API on the command line ("dotnet run" in the RestApi dir)
        - Run or debug the IntegrationTests


    