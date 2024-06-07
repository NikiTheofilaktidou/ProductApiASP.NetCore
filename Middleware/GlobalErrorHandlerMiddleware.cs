using Newtonsoft.Json;

namespace ProductApi.Middleware
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception (you can customize logging based on your requirements)

                // Set the response status code
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // Return a JSON response with error details
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    message = "An error occurred while processing your request.",
                    error = ex.Message // Include more details if needed
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
    }
}
