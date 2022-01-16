using System;
using System.Text.Json;
using System.Threading.Tasks;
using GeospatialLocation.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace GeospatialLocation.API.Extensions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
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
                case BusinessException businessException:
                    Log.Information(
                        $"Domain Exception: {businessException.Code} - {businessException.Message}",
                        businessException.PropertyValues);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return context.Response.WriteAsync(
                        JsonSerializer.Serialize(
                            new Error(
                                businessException.Code, businessException.Message,
                                businessException.PropertyValues)));
            }

            var error = new Error("An error occurred during action handling", exception.Message);
            Log.Error(exception, error.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
}