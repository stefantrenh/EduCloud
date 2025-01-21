using EduCloud.API.Common.ApiResponse;
using EduCloud.API.Common.Exceptions;
using EduCloud.Application.Common.Exceptions;
using System.Net;

namespace EduCloud.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    ConflictException => (int)HttpStatusCode.Conflict,
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var errorResponse = new ApiResponse<object>(
                    context.Response.StatusCode,
                    ex.Message,
                    data: null 
                );

                if (_environment.IsDevelopment())
                {
                    errorResponse.Data = new { ex.StackTrace };
                }

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
