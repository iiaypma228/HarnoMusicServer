using System.Net;

namespace Server.API;

public class ErrorMiddleware
{
     private readonly RequestDelegate next;

        public ErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IHostEnvironment env /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType =  "text/plain";
                await context.Response.WriteAsync(ex.Message);
            }
        }
}