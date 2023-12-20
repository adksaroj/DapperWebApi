using Microsoft.AspNetCore.Mvc;

namespace DapperWebApi.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
                _logger.LogError(message: $"TraceId : {context.TraceIdentifier} {ex.Message} {ex.StackTrace}");
                await HandleExceptionAsync(context, ex);
			}
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Detail = $"Trace Id {context.TraceIdentifier}"
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
