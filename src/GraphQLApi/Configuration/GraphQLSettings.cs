using GraphQL.Types;
using Microsoft.AspNetCore.Http;

namespace GraphQLApi.Configuration
{
    public class GraphQLSettings
    {
        public PathString Path { get; set; } = "/api/graphql";
        public ISchema Schema { get; set; }
    }
}
