using Expenses.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Expenses.Api.Middlewares
{
    // https://stackoverflow.com/questions/58821308/asp-net-core-exception-handling-middleware
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static Task HandleException(HttpContext context, Exception ex)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected

            // Specify different custom exceptions here
            //if (ex is NotFoundException) code = HttpStatusCode.BadRequest;

            //string result = JsonConvert.SerializeObject(new { error = ex.Message });

            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)code;

            //return context.Response.WriteAsync(result);

            if (ex is NotFoundException) code = HttpStatusCode.NotFound;
            context.Response.StatusCode = (int)code;

            return Task.CompletedTask;
        }
    }
}
