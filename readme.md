# dotnetcore-api

Prototype .NET Core API implementation (work in progress).

~~Check out the API on Azure App Services.~~

## Prerequisites

- [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/core)
- [Docker](https://www.docker.com) to run dependencies ([IdentityServer4](https://identityserver.io) and [SQL Server](https://www.microsoft.com/en-us/sql-server)) locally. Please note that [Microsoft SQL Server on Linux for Docker](https://hub.docker.com/r/microsoft/mssql-server-linux/) requires at least 3.25 GB of RAM.

## Running locally

- ``git pull git@github.com:robyvandamme/dotnetcore-api.git``
- ``cd dotnetcore-api``
- ``docker-compose up -d``
- ``dotnet restore``
- ``dotnet run -p src/RestApi/RestApi.csproj --server.urls=http://localhost:8081``
- The API should be available at ``http://localhost:8081/swagger``

## Authenticating

- ``alice`` - ``password``
- ``bob`` - ``password``

## Dev Dependencies

- [NodeJS with NPM](https://nodejs.org/en/) and [Autorest](https://github.com/Azure/autorest) if you want to regenerate the integration test clients.
- Microsoft SQL Server with the AdventureWorks database: [Docker image](https://hub.docker.com/r/robyvandamme/mssql-server-linux-adventureworks/) and [Github repo](https://github.com/robyvandamme/adventureworks-docker)
- IdentityServer4: [Docker image](https://hub.docker.com/r/robyvandamme/identityserver4-demo/) and [GitHub repo](https://github.com/robyvandamme/identityserver4-demo)

## Technical Features

### General

- Dependency injection using [Autofac](https://autofac.org/)
- Configurable logging using [Serilog](https://serilog.net)
- API documentation generation [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) and [Swagger/ Open API](https://swagger.io)
- ~~[Dapper](https://github.com/StackExchange/Dapper) and [Dapper Contrib](https://github.com/StackExchange/Dapper/tree/master/Dapper.Contrib)~~ [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore) for SQL Server data access
- [Automapper](http://automapper.org/) for object mapping
- Authentication and authorization using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4)
- Async

### REST API

- Multiple access levels (admin, public)
- Configurable CORS
- Configurable rate-limiting
- Global exception handling and logging
- Request logging
- HTTP caching using both Entity Tags and Response Cache
- GET, POST, PUT, DELETE, PATCH
- GET with paging, ordering, searching, filtering and customizable result fields

### Testing

- API integration tests using a strongly typed [Autorest](https://github.com/Azure/AutoRest) API client.
- Unit tests and integration tests using [xUnit](https://xunit.github.io)

### Code Quality

- Code analysis using [StyleCop](https://github.com/StyleCop)

### Build and deployment

~~Continuous integration and delivery to Azure App Services using [Visual Studio Team Services](https://www.visualstudio.com/team-services/).~~

## Integration tests

- For now not using the [integration test server](https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing), but just running the api and running the tests against that since that is the setup for CI as well.
- To use the correct config for dev (local), make sure the enviroment is set correctly to Development before running the tests. (https://dotnetcoretutorials.com/2017/05/03/environments-asp-net-core/)

## Generating AutoRest clients

- ``npm run gen-public``
- ``npm run gen-admin``
