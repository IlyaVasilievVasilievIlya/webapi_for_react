using Cars.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cars.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ProcessProblemDetails? response = null;
            try
            {
                await next.Invoke(context);
            }
            catch (ProcessException pe)
            {
                response = pe.Details ?? new ProcessProblemDetails() { Title = "unknown error"};
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            catch (Exception pe)
            {
                response = new ProcessProblemDetails() { Title = pe.Message};
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            finally
            {
                if (response is not null)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    await context.Response.StartAsync();
                    await context.Response.CompleteAsync();
                }
            }
        }
    }
}
