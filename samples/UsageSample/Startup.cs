using Aurokk.LoggingMiddleware.Extensions;
using Aurokk.LoggingMiddleware.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

// TODO: нагет
// TODO: хорошая дока
namespace UsageSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                // Add this one and configure what fields you want to log
                .AddHttpContextLogging(new LoggingMiddlewareSettings
                {
                    AddRequestLog = true,
                    RequestLogFields = new RequestLogFieldsSettings
                    {
                        Request = new RequestLogFields
                        {
                            Body = true,
                            ContentType = true,
                            Cookies = true,
                            Headers = true,
                            Host = true,
                            IsHttps = true,
                            Method = true,
                            Path = true,
                            Protocol = true,
                            Query = true,
                            QueryString = true,
                            Scheme = true,
                            RemoteIpAddress = true,
                        }
                    },
                    AddResponseLog = true,
                    ResponseLogFields = new ResponseLogFieldsSettings
                    {
                        Request = new RequestLogFields
                        {
                            Body = true,
                            ContentType = true,
                            Cookies = true,
                            Headers = true,
                            Host = true,
                            IsHttps = true,
                            Method = true,
                            Path = true,
                            Protocol = true,
                            Query = true,
                            QueryString = true,
                            Scheme = true,
                            RemoteIpAddress = true,
                        },
                        Response = new ResponseLogFields
                        {
                            Body = true,
                            ContentType = true,
                            Headers = true,
                            StatusCode = true,
                        }
                    },
                    AddContext = true,
                    ContextFields = new ContextFieldsSettings
                    {
                        Request = new RequestLogFields
                        {
                            Body = true,
                            ContentType = true,
                            Cookies = true,
                            Headers = true,
                            Host = true,
                            IsHttps = true,
                            Method = true,
                            Path = true,
                            Protocol = true,
                            Query = true,
                            QueryString = true,
                            Scheme = true,
                            RemoteIpAddress = true,
                        },
                    },
                })
                .AddMvc(options => { options.Filters.Add(typeof(UnhandledExceptionFilter)); });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseStaticFiles()
                // Add this one before mvc and after static files
                .UseHttpContextLogging()
                .UseMvc();
        }
    }
}