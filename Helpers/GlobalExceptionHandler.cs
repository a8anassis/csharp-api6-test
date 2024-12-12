namespace SchoolApp.Helpers
{
    using System.Net;
    using System.Text.Json;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SchoolApp.Exceptions;

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An exception occurred while processing the request.");

            var response = httpContext.Response;
            response.ContentType = "application/json";

            // Map exception types to HTTP status codes
            response.StatusCode = exception switch
            {
                InvalidRegistrationException or EntityAlreadyExistsException => (int)HttpStatusCode.BadRequest,   // 400
                EntityNotAuthorizedException => (int)HttpStatusCode.Unauthorized,    // 401
                EntityForbiddenException => (int)HttpStatusCode.Forbidden,           // 403
                EntityNotFoundException => (int)HttpStatusCode.NotFound,             // 404
                _ => (int)HttpStatusCode.InternalServerError                        // 500
            };

            // Serialize the error response to an anonymous object
            var errorResponse = new
            {
                message = exception.Message,
                statusCode = response.StatusCode
            };

            await response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true; // Indicate that the exception has been handled
        }
    }

}
