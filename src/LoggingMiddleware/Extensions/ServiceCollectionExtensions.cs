using System;
using Aurokk.LoggingMiddleware.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Aurokk.LoggingMiddleware.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpContextLogging(this IServiceCollection collection,
            LoggingMiddlewareSettings settings = null)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection
                .AddSingleton(settings ?? new LoggingMiddlewareSettings())
                .AddSingleton<LoggingMiddleware>();
        }
    }
}