using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace GeospatialLocation.API.Extensions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                //case BusinessException businessException:
                //    Log.Information(
                //        $"Domain Exception: {businessException.Code} - {businessException.Message}",
                //        businessException.PropertyValues);
                //    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                //    return context.Response.WriteAsync(
                //        JsonSerializer.Serialize(
                //            new Error(
                //                businessException.Code, businessException.Message,
                //                businessException.PropertyValues)));

                //case EntityAlreadyExistsException entityAlreadyExistsException:
                //    Log.Information(entityAlreadyExistsException.Description);
                //    context.Response.StatusCode = StatusCodes.Status409Conflict;
                //    return context.Response.WriteAsync(
                //        JsonSerializer.Serialize(
                //            new Error(
                //                entityAlreadyExistsException.Message,
                //                entityAlreadyExistsException.Description)));

                //case EntityNotFoundException entityNotFoundException:
                //    Log.Information(entityNotFoundException.Description);
                //    context.Response.StatusCode = StatusCodes.Status404NotFound;
                //    return context.Response.WriteAsync(
                //        JsonSerializer.Serialize(
                //            new Error(
                //                entityNotFoundException.Message,
                //                entityNotFoundException.Description)));
            }

            var error = new Error("An error occurred during action handling", exception.Message);
            Log.Error(exception, error.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
}