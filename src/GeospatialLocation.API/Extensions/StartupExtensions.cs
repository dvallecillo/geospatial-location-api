using GeospatialLocation.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace GeospatialLocation.API.Extensions
{
    public static class StartupExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            return app;
        }
    }
}