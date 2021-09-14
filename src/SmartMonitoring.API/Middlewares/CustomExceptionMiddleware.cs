using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmartMonitoring.API.Models.Responses;
using SmartMonitoring.Business.Exceptions;
using SmartMonitoring.Domain.Exceptions;
using System;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMonitoring.API.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private const string DEFAULT_ERROR_MESSAGE = "An unexpected error occurred.";

        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException ex)
            {
                await HandleErrorException(httpContext, ex, HttpStatusCode.BadRequest);
            }
            catch (ServiceNotFoundException ex)
            {
                await HandleErrorException(httpContext, ex, HttpStatusCode.NotFound);
            }
            catch (ServiceNameAlreadyInUseException ex)
            {
                await HandleErrorException(httpContext, ex, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                await HandleCriticalException(httpContext, ex);
            }
        }

        private async Task HandleErrorException(HttpContext httpContext, Exception ex, HttpStatusCode statusCode)
        {
            _logger.LogError(ex, ex.Message);
            await FormatResponseAsync(httpContext, ex.Message, statusCode);
        }

        private async Task HandleCriticalException(HttpContext httpContext, Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            await FormatResponseAsync(httpContext, DEFAULT_ERROR_MESSAGE, HttpStatusCode.InternalServerError);
        }

        private async Task FormatResponseAsync(HttpContext httpContext, string message, HttpStatusCode statusCode)
        {
            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;

            var errorResponse = new ErrorResponse(message);
            var jsonErrorResponse = Serialize(errorResponse);

            await httpContext.Response.WriteAsync(jsonErrorResponse);
        }

        private string Serialize<T>(T response)
        {
            return JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
