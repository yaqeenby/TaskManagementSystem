using System.Text.Json;

namespace TaskManagementSystem.Middkewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                // Handle common status codes like 401, 403, 404
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await WriteResponse(context, 401, "Unauthorized: You must be logged in.");
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await WriteResponse(context, 403, "Forbidden: You don't have permission.");
                }
                else if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await WriteResponse(context, 404, "Not Found: Resource does not exist.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                await WriteResponse(context, 500, "Internal Server Error: Something went wrong.");
            }
        }

        private Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                statusCode,
                message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
