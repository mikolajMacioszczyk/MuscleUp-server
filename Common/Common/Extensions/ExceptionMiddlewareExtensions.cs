using Common.Helpers;
using Microsoft.AspNetCore.Builder;

namespace Common.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseDeveloperExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<DeveloperExceptionMiddleware>();
        }

        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
