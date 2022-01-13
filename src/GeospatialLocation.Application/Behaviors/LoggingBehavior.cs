using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeospatialLocation.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request!.GetGenericTypeName();
            var requestType = requestName.EndsWith("Query") ? "Query" : "Command";

            _logger.LogInformation(
                "--- Handling {RequestType} {RequestName} ({@Request})", requestType, requestName,
                request);
            var response = await next();
            _logger.LogInformation(
                "--- {RequestType} {RequestName} handled", requestType, requestName);

            return response;
        }
    }
}