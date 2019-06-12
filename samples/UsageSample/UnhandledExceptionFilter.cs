using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace UsageSample
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<UnhandledExceptionFilter> _logger;

        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _logger.LogError(context.Exception, "Unexpected exception was occured!");

            context.Result = new ObjectResult(new
            {
                message = "Unhandled exception has occured!",
            })
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}