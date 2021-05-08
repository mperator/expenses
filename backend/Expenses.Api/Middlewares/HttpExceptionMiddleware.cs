using Expenses.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Expenses.Api.Middlewares
{
    // https://stackoverflow.com/questions/58821308/asp-net-core-exception-handling-middleware
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ProblemDetailsFactory _factory;
        private readonly IActionResultExecutor<ObjectResult> _executor;

        public HttpExceptionMiddleware(RequestDelegate next, ProblemDetailsFactory factory, IActionResultExecutor<ObjectResult> executor)
        {
            _next = next;
            _factory = factory;
            _executor = executor;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        // https://stackoverflow.com/questions/54104138/mediatr-fluent-validation-response-from-pipeline-behavior
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (ex is FluentValidation.ValidationException vex)
            {
                var result = JsonConvert.SerializeObject(vex.Errors);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(result);
                await context.Response.CompleteAsync();
            }
            else if(ex is ValidationException ve)
            {
                var details = _factory.CreateProblemDetails(context, (int)HttpStatusCode.BadRequest, "BusinessRule violation.", detail: ve.Message);

                var result = new ObjectResult(details);

                var routeData = context.GetRouteData() ?? new RouteData(); 
                var actionContext = new ActionContext(context, routeData, new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
                await _executor.ExecuteAsync(actionContext, result);
                await context.Response.CompleteAsync();
            }
            else if (ex is Application.Common.Exceptions.NotFoundException) code = HttpStatusCode.NotFound;
            else
            {
                context.Response.StatusCode = (int)code;
            }
        }
    }
}
