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

                var errorResponse = new
                {
                    statusCode = context.Response.StatusCode,
                    message = ex.Message,
                    errorCode = ex.GetType().Name.ToUpper(),
                    stackTrace = _environment.IsDevelopment() ? ex.StackTrace : null
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }


}
