using Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Api
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseETags(this IApplicationBuilder app)
        {
            app.UseMiddleware<ETagMiddleware>();
        }

        public static void UseCustomtExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
