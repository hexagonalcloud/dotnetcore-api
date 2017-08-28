# dotnetcore-api

Prototype .NET Core API implementation (work in progress)

## Prerequisites

- .[NET Core 2.0](https://www.microsoft.com/net/download/core)
- [Autorest](https://github.com/Azure/autorest) if you want to regenerate the integration test client.
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) if you want to run the db locally. The db is the AdventureWorks sample that is available when you set up a SQL Azure database, more details [here](https://blogs.msdn.microsoft.com/kaevans/2015/03/06/adventure-works-for-azure-sql-database/).
- [Redis](https://redis.io/) if you want to run a Redis cache locally, but you can also just use the in-memory cache.

## Parts

- [Autofac](https://autofac.org/) for dependency injection.
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) for API documentation generation.
- API Integration tests using a strongly typed [Autorest](https://github.com/Azure/AutoRest) API client.
- [Dapper](https://github.com/StackExchange/Dapper) and [Dapper Contrib](https://github.com/StackExchange/Dapper/tree/master/Dapper.Contrib) for SQL Server data access.
- [Redis](https://redis.io/) cache.
- Authentication and authorizaton using [IdentityServer4](https://github.com/IdentityServer/IdentityServer4).
- CI and CD to [Azure App Services](https://azure.microsoft.com/en-us/services/app-service/) using [VSTS](https://www.visualstudio.com/team-services/).


## Deployed API on Azure
- [Api](https://dotnetcore-api.azurewebsites.net/swagger)


## Integration tests
- For now not using the [integration test server](https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing), but just running the api and running the tests against that since that is the setup for CI as well.
- To use the correct config for dev (local), make sure the enviroment is set correctly to Development before running the tests. (https://dotnetcoretutorials.com/2017/05/03/environments-asp-net-core/)

## Setup
- Adapt __src/Api/appsettings.Development.json__ and __test/IntegrationTests/testsettings.Development.json__ to match you local development environment.
- To install the sample db, run __db/setup-db.sql__
- To setup a minimal IdentityServer to match the currently used authorization scheme,  follow the steps in this [quickstart](http://docs.identityserver.io/en/release/quickstarts/1_client_credentials.html), but you can also just disable auhtorization in the config. Do note that if you disable authorization some of the integration tests will fail.
- Make sure the Api runs on port 5001
    - add "server.urls=http://localhost:5001" to the program args
    - dotnet run --server.urls=http://localhost:5001





    



