using System;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BookStore.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            try
            {
                string message = "[Request] HTTP - " + context.Request.Method + " - " + context.Request.Path;
                Console.WriteLine(message);
                await _next(context); // after this process is finished it will continue to below this line.
                watch.Stop();
                message = "[RESPONSE] HTTP - " + context.Request.Method + " - " + context.Request.Path + "responded " +
                          context.Response.StatusCode + "in " + watch.Elapsed.TotalMilliseconds + " ms";
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                watch.Stop();
                await HandleException(context, ex, watch);
            }
          
        }

        private Task HandleException(HttpContext context, Exception ex, Stopwatch watch)
        {
            string message = "[ERROR] HTTP " + context.Request.Method + " - " + context.Response.StatusCode +
                             "Error Message" + ex.Message + "in" + watch.Elapsed.TotalMilliseconds;
            
            Console.WriteLine(message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new { error = ex.Message }, Formatting.None);
            return context.Response.WriteAsync(result);

        }
    }

    public static class CustomExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}