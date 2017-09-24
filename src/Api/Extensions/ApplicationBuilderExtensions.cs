using Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Api
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseETags(this IApplicationBuilder app)
        {
            app.UseMiddleware<ETags>();
        }
    }
}
