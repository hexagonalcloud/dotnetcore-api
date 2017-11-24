using Microsoft.AspNetCore.Builder;
using RestApi.Middleware;

namespace RestApi.Extensions
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
