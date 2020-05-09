using IronSoccerDDD.Api.Utils;
using IronSoccerDDD.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;

namespace IronSoccerDDD.Api.Handlers
{
    public static class ExceptionHandlerMiddleware
    {
        private static ILogger<Startup> _logger;

        public static IApplicationBuilder UseIronExceptionHandler(
            this IApplicationBuilder builder,
            ILogger<Startup> logger)
        {
            _logger = logger;

            return builder.UseExceptionHandler(HandlerHttpExceptions);
        }

        static void HandlerHttpExceptions(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature?.Error;

                _logger.LogError("Handler Exception", exception);
                string message = exception.Message;

                if (exception is ArgumentException ||
                    exception is BusinessException)
                    context.Response.StatusCode = 400;
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Internal Error";
                }
                    
                string result = JsonConvert.SerializeObject(Envelope.Error(exception.Message));
                await context.Response.WriteAsync(result);
            });
        }
    }
}
