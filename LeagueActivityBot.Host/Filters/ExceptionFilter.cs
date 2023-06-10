using System;
using System.Net;
using JetBrains.Annotations;
using LeagueActivityBot.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LeagueActivityBot.Host.Filters
{
    [UsedImplicitly]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException([NotNull] ExceptionContext context)
        {
            var ex = context.Exception;
            switch (ex)
            {
                case ApiResponseException apiResponseException:
                    SetResponse(context, apiResponseException.HttpStatusCode);
                    break;
                case ApplicationException _:
                case Exception _:
                    SetResponse(context, HttpStatusCode.InternalServerError);
                    _logger.LogError(ex.Message, context.Exception);
                    break;
            }
        }
        
        private static void SetResponse(ExceptionContext context, HttpStatusCode code)
        {
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new ObjectResult(new {message = context.Exception.Message});
            context.ExceptionHandled = true;
        }
    }
}