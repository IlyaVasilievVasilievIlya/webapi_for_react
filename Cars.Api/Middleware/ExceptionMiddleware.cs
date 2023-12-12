using Cars.Api.Exceptions;
using System.Text.Json;

namespace Cars.Api.Middleware
{
    /// <summary>
    /// промежутное ПО обработки ошибок
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="next">сл. часть конвейера</param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// метод middleware
        /// </summary>
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
