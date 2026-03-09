using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using University.Core.Exceptions;

namespace UniverSity.Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if (context.Exception is NotFoundException)
            {
                _logger.LogWarning(exception, "Resource not found");

                context.Result = Response(exception.Message, "Resource not found", StatusCodes.Status404NotFound);
                return;
            }
            if (exception is BusinessException businessException)
            {

                if (businessException.Errors != null && businessException.Errors.Any())

                    context.Result = Response(businessException.Errors, "One or more business validation errors occurred", StatusCodes.Status400BadRequest);

                else
                    context.Result = Response(businessException.Message, "One or more business validation errors occurred", StatusCodes.Status400BadRequest);

                _logger.LogWarning(exception, "Resource not found");

                return;
            }
            if (exception is ArgumentNullException)
            {
                context.Result = Response(exception.Message, "Missing data", StatusCodes.Status400BadRequest);
                return;
            }
            if (exception is UnauthorizedAccessException)
            {
                context.Result = Response(exception.Message, "Access denied", StatusCodes.Status403Forbidden);
                return;
            }

            _logger.LogError(exception, "Unknown error occurred");
            context.Result = Response(exception.Message, "Internal server error", StatusCodes.Status500InternalServerError, exception.StackTrace);
        }

        public ObjectResult Response(string message, string responseException, int status, string? stackTrace = null)
        {
            var result =
                new ApiResponse
                {
                    StatusCode = status,
                    Message = message,
                    ResponseException = responseException,
                    IsError = true,
                    Version = "1.0",
                    Result = stackTrace
                };
            return new ObjectResult(result)
            {
                StatusCode = status
            };
        }

        public ObjectResult Response(Dictionary<string, List<string>> errors, string responseException, int status, string? stackTrace = null)
        {
            var result =
                new ApiResponse
                {
                    StatusCode = status,
                    Message = responseException,
                    ResponseException = responseException,
                    IsError = true,
                    Version = "1.0",
                    Result = errors
                };
            return new ObjectResult(result)
            {
                StatusCode = status
            };
        }
    }
    }
