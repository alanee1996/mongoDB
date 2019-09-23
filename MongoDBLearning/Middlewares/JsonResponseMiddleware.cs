using Base;
using Base.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MongoDBLearning.Middlewares
{
    public class JsonResponseMiddleware
    {
        private readonly RequestDelegate _next;
        public JsonResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == 404) throw new NotFoundException($"The request for {httpContext.Request.Path} not found");
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionNotFound(httpContext, ex);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            //var customException = exception as BaseCustomException;
            var statusCode = (int)HttpStatusCode.InternalServerError;

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(JsonResponse.failed(exception.Message, statusCode)));
        }

        private async Task HandleExceptionNotFound(HttpContext context, NotFoundException exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var status = (int)HttpStatusCode.NotFound;
            response.StatusCode = status;
            await response.WriteAsync(JsonConvert.SerializeObject(JsonResponse.failed(exception.Message, status)));
        }

    }

    public static class JsonResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseJsonResponse(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsonResponseMiddleware>();
        }
    }
}
