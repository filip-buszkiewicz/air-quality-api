using System.Net;

namespace AirQualityApi.Exceptions
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex) 
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                context.Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                await context.Response.WriteAsync(ex.Message);
            }

            catch (Exception ex) 
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
        }
    }
}
