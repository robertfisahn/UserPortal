using Newtonsoft.Json;
using UserPortal.Exceptions;

namespace UserPortal.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new { error = badRequestException.Message });
                await context.Response.WriteAsync(result);
            }
        }

    }
}
