# Aurokk.LoggingMiddleware

Middleware allows you to log incoming/outcoming requests with configurable fields set. 
Also it allows you to add a lot of request fields to logging context of http request.

## Log example

```
{
    ...
    "HttpContext": {
        "Request": {
            "RemoteIpAddress": "::1",
            "ContentType": null,
            "Method": "GET",
            "Protocol": "HTTP/1.1",
            "IsHttps": false,
            "Scheme": "http",
            "Host": "localhost:5000",
            "Path": "/exception",
            "QueryString": "",
            "Query": {},
            "Headers": {
                "Connection": "keep-alive",
                "Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3",
                "Accept-Encoding": "gzip, deflate, br",
                "Accept-Language": "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7",
                "Cookie": "_ga=GA1.1.307182432.1559671987; amplitude_id_6a71b2e4117775c7f74033b4e234fdda=eyJkZXZpY2VJZCI6IjliMjE1MGRmLTM0ZjYtNGEzMC04NDAwLTZjZmRjNjczMThlYlIiLCJ1c2VySWQiOm51bGwsIm9wdE91dCI6ZmFsc2UsInNlc3Npb25JZCI6MTU2MDM2MDQ1MzA0MCwibGFzdEV2ZW50VGltZSI6MTU2MDM2MTAzNDEyOSwiZXZlbnRJZCI6MiwiaWRlbnRpZnlJZCI6MCwic2VxdWVuY2VOdW1iZXIiOjJ9",
                "Host": "localhost:5000",
                "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36",
                "Upgrade-Insecure-Requests": "1"
            },
            "Cookies": {
                "_ga": "GA1.1.307182432.1559671987",
                "amplitude_id_6a71b2e4117775c7f74033b4e234fdda": "eyJkZXZpY2VJZCI6IjliMjE1MGRmLTM0ZjYtNGEzMC04NDAwLTZjZmRjNjczMThlYlIiLCJ1c2VySWQiOm51bGwsIm9wdE91dCI6ZmFsc2UsInNlc3Npb25JZCI6MTU2MDM2MDQ1MzA0MCwibGFzdEV2ZW50VGltZSI6MTU2MDM2MTAzNDEyOSwiZXZlbnRJZCI6MiwiaWRlbnRpZnlJZCI6MCwic2VxdWVuY2VOdW1iZXIiOjJ9"
            },
            "Body": ""
        }
    }
    ...
}
```

## Settings

You can enable/disable logging of any field in each type of logs using extension and you can configure nesting and names of layers.

```
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            // Add this one and configure what fields you want to log
            .AddHttpContextLogging(new LoggingMiddlewareSettings());
    }
}
```