using Expenses.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

        // https://stackoverflow.com/questions/54104138/mediatr-fluent-validation-response-from-pipeline-behavior
        private static Task HandleException(HttpContext context, Exception ex)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (ex is FluentValidation.ValidationException vex)
            {
                var result = JsonConvert.SerializeObject(vex.Errors);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(result);
            }

            if (ex is NotFoundException) code = HttpStatusCode.NotFound;
            context.Response.StatusCode = (int)code;

            return Task.CompletedTask;
        }
    }
}
