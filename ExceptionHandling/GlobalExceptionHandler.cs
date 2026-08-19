using Microsoft.AspNetCore.Diagnostics;

namespace CarRental.API.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(
          ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
          HttpContext httpContext,
          Exception exception,
          CancellationToken cancellationToken)
        {
            // Log the unexpected exception
            _logger.LogError(
                exception,
                "An unexpected error occurred.");

            // Set HTTP status code
            httpContext.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            // Return safe response to the client
            await httpContext.Response.WriteAsJsonAsync(
                new
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    message = "An unexpected error occurred."
                },
                cancellationToken);

            return true;
        }
    }
}
