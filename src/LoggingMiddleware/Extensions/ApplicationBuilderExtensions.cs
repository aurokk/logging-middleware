using System;
using Microsoft.AspNetCore.Builder;

namespace Aurokk.LoggingMiddleware.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpContextLogging(this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}