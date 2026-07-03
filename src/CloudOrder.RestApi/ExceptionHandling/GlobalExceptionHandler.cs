using CloudOrder.Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudOrder.RestApi.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            ProblemDetails? problem = exception switch
            {
                NotFoundException => new ProblemDetails
                {
                    Title = "Resource Not Found",
                    Detail = exception.Message,
                    Status = StatusCodes.Status404NotFound
                },
                BusinessException => new ProblemDetails
                {
                    Title = "Business Rule Violation",
                    Detail = exception.Message,
                    Status = StatusCodes.Status400BadRequest
                },
                _ => new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Detail = "Please try again later.",
                    Status = StatusCodes.Status500InternalServerError
                }
            };

            httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }
    }
}
