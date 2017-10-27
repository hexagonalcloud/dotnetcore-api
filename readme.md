# dotnetcore-api

Prototype .NET Core API implementation (work in progress)

## Prerequisites

- [.NET Core 2.0](https://www.microsoft.com/net/download/core)
- [Autorest](https://github.com/Azure/autorest) if you want to regenerate the integration test client.
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) if you want to run the db locally. The db is the AdventureWorks sample that is available when you set up a SQL Azure database, more details [here](https://blogs.msdn.microsoft.com/kaevans/2015/03/06/adventure-works-for-azure-sql-database/).
- ~~[Redis](https://redis.io/) if you want to run a Redis cache locally, but you can also just use the in-memory cache~~

## Technical Features

### General

- Dependency injection using [Autofac](https://autofac.org/)
- Configurable logging using [Serilog](https://serilog.net)
- API documentation generation [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) and [Swagger/ Open API](https://swagger.io)
- ~~[Dapper](https://github.com/StackExchange/Dapper) and [Dapper Contrib](https://github.com/StackExchange/Dapper/tree/master/Dapper.Contrib)~~ [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore) for SQL Server data access
- [Automapper](http://automapper.org/) for object mapping
- Authentication and authorization using [JWT](https://en.wikipedia.org/wiki/JSON_Web_Token) with [IdentityServer4](https://github.com/IdentityServer/IdentityServer4)

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

- Continuous integration and delivery to [Azure App Service](https://azure.microsoft.com/en-us/services/app-service/) using [Visual Studio Team Services](https://www.visualstudio.com/team-services/).

## API on Azure App Service

- [.NET Core API](https://dotnetcore-api.azurewebsites.net/swagger)

## Setup
- Adapt __src/Api/appsettings.Development.json__ and __test/IntegrationTests/testsettings.Development.json__ to match you local development environment.
- To install the sample db, run __db/setup-db.sql__
- To setup a minimal IdentityServer to match the currently used authorization scheme, follow the steps in this [quickstart](http://docs.identityserver.io/en/release/quickstarts/1_client_credentials.html).
- Make sure the Api runs on port 5001
    - add "server.urls=http://localhost:5001" to the program args
    - dotnet run --server.urls=http://localhost:5001

## Integration tests
- For now not using the [integration test server](https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing), but just running the api and running the tests against that since that is the setup for CI as well.
- To use the correct config for dev (local), make sure the enviroment is set correctly to Development before running the tests. (https://dotnetcoretutorials.com/2017/05/03/environments-asp-net-core/)
