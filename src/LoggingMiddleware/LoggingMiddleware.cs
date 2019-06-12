using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurokk.LoggingMiddleware.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;

namespace Aurokk.LoggingMiddleware
{
    public class LoggingMiddleware : IMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly LoggingMiddlewareSettings _settings;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger, LoggingMiddlewareSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestValues = await GetRequestValues(context);
            if (_settings.AddRequestLog)
            {
                var requestLogScope = CreateRequestLogScope(requestValues);
                using (_logger.BeginScope(requestLogScope))
                {
                    _logger.LogInformation("Incoming request started {Path}", requestValues.Path);
                }
            }

            if (_settings.AddContext)
            {
                var contextScope = CreateContextScope(requestValues);
                using (_logger.BeginScope(contextScope))
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }

            var responseValues = await GetResponseValues(context);
            if (_settings.AddResponseLog)
            {
                var responseLogScope = CreateResponseLogScope(requestValues, responseValues);
                using (_logger.BeginScope(responseLogScope))
                {
                    _logger.LogInformation("Incoming request ended {Path}", requestValues.Path);
                }
            }
        }

        #region Calculate request values

        public async Task<RequestValues> GetRequestValues(HttpContext context)
        {
            return new RequestValues
            {
                RemoteIpAddress = context.Connection.RemoteIpAddress.ToString(),
                ContentType = context.Request.ContentType,
                Method = context.Request.Method,
                Protocol = context.Request.Protocol,
                Host = context.Request.Host.ToString(),
                Path = context.Request.Path.ToString(),
                IsHttps = context.Request.IsHttps,
                Scheme = context.Request.Scheme,
                QueryString = context.Request.QueryString.ToString(),
                Query = context.Request.Query.ToDictionary(x => x.Key, y => y.Value.ToString()),
                Headers = context.Request.Headers.ToDictionary(x => x.Key, y => y.Value.ToString()),
                Cookies = context.Request.Cookies.ToDictionary(x => x.Key, y => y.Value.ToString()),
                Body = await GetRequestBody(context),
            };
        }

        private static async Task<string> GetRequestBody(HttpContext context)
        {
            var body = string.Empty;

            switch (context.Request.ContentLength)
            {
                case null:
                case 0:
                {
                    return body;
                }

                default:
                {
                    context.Request.EnableRewind();

                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                    {
                        body = await reader.ReadToEndAsync();
                    }

                    context.Request.Body.Seek(0, SeekOrigin.Begin);

                    return body;
                }
            }
        }

        #endregion

        #region Calculate response values

        private static async Task<ResponseValues> GetResponseValues(HttpContext context)
        {
            return new ResponseValues
            {
                StatusCode = context.Response.StatusCode,
                ContentType = context.Response.ContentType,
                Headers = context.Response.Headers.ToDictionary(x => x.Key, y => y.Value.ToString()),
                Body = await GetResponseBody(context),
            };
        }

        private static async Task<string> GetResponseBody(HttpContext context)
        {
            var body = string.Empty;

            switch (context.Request.ContentLength)
            {
                case null:
                case 0:
                {
                    return body;
                }

                default:
                {
                    using (var reader = new StreamReader(context.Response.Body, Encoding.UTF8, true, 1024, true))
                    {
                        body = await reader.ReadToEndAsync();
                    }

                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    return body;
                }
            }
        }

        #endregion

        #region Create request log scope

        private Dictionary<string, object> CreateRequestLogScope(RequestValues requestValues)
        {
            var mappedRequestValues = MapAndFilterRequestValues(requestValues);
            var namespacedRequestValues = NamespaceValues(mappedRequestValues, _settings.RequestFieldsNamespace);
            var requestLogScope = NamespaceValues(namespacedRequestValues, _settings.AllFieldsNamespace);
            return requestLogScope;
        }

        private Dictionary<string, object> MapAndFilterRequestValues(RequestValues requestValues)
        {
            var values = _settings.RequestLogFields.Request.GetType().GetProperties()
                .Where(p => (bool) p.GetValue(_settings.RequestLogFields.Request, null))
                .ToDictionary(
                    p => PrefixRequestField(p.Name),
                    p => requestValues.GetType().GetProperty(p.Name)?.GetValue(requestValues, null)
                );

            return values;
        }

        #endregion

        #region Create context scope

        private Dictionary<string, object> CreateContextScope(RequestValues requestValues)
        {
            var mappedRequestValues = MapAndFilterContextValues(requestValues);
            var namespacedRequestValues = NamespaceValues(mappedRequestValues, _settings.RequestFieldsNamespace);
            var contextScope = NamespaceValues(namespacedRequestValues, _settings.AllFieldsNamespace);
            return contextScope;
        }

        private Dictionary<string, object> MapAndFilterContextValues(RequestValues requestValues)
        {
            var values = _settings.ContextFields.Request.GetType().GetProperties()
                .Where(p => (bool) p.GetValue(_settings.ContextFields.Request, null))
                .ToDictionary(
                    p => PrefixRequestField(p.Name),
                    p => requestValues.GetType().GetProperty(p.Name)?.GetValue(requestValues, null)
                );

            return values;
        }

        #endregion

        #region Create response log scope

        private Dictionary<string, object> CreateResponseLogScope(
            RequestValues requestValues,
            ResponseValues responseValues)
        {
            var mappedRequestValues = MapAndFilterResponseValues(requestValues);
            var namespacedRequestValues = NamespaceValues(mappedRequestValues, _settings.RequestFieldsNamespace);

            var mappedResponseValues = MapAndFilterResponseValues(responseValues);
            var namespacedResponseValues = NamespaceValues(mappedResponseValues, _settings.ResponseFieldsNamespace);

            var mergedValues = namespacedRequestValues
                .Union(namespacedResponseValues)
                .ToDictionary(x => x.Key, x => x.Value);
            var responseLogScope = NamespaceValues(mergedValues, _settings.AllFieldsNamespace);
            return responseLogScope;
        }

        private Dictionary<string, object> MapAndFilterResponseValues(RequestValues requestValues)
        {
            var values = _settings.ResponseLogFields.Request.GetType().GetProperties()
                .Where(p => (bool) p.GetValue(_settings.ResponseLogFields.Request, null))
                .ToDictionary(
                    p => PrefixRequestField(p.Name),
                    p => requestValues.GetType().GetProperty(p.Name)?.GetValue(requestValues, null)
                );

            return values;
        }

        private Dictionary<string, object> MapAndFilterResponseValues(ResponseValues responseValues)
        {
            var values = _settings.ResponseLogFields.Response.GetType().GetProperties()
                .Where(p => (bool) p.GetValue(_settings.ResponseLogFields.Response, null))
                .ToDictionary(
                    p => PrefixResponseField(p.Name),
                    p => responseValues.GetType().GetProperty(p.Name)?.GetValue(responseValues, null)
                );

            return values;
        }

        #endregion

        #region Commons

        private string PrefixRequestField(string fieldName)
        {
            var normalizedPrefix = _settings.RequestFieldsPrefix ?? string.Empty;
            return $"{normalizedPrefix}{fieldName}";
        }

        private string PrefixResponseField(string fieldName)
        {
            var normalizedPrefix = _settings.ResponseFieldsPrefix ?? string.Empty;
            return $"{normalizedPrefix}{fieldName}";
        }

        private static Dictionary<string, object> NamespaceValues(Dictionary<string, object> values, string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace))
            {
                return values;
            }

            return new Dictionary<string, object>
            {
                {@namespace, values},
            };
        }

        #endregion
    }
}