using System.Collections.Generic;

namespace Aurokk.LoggingMiddleware
{
    public class RequestValues
    {
        public string RemoteIpAddress { get; set; }
        public string ContentType { get; set; }
        public string Method { get; set; }
        public string Protocol { get; set; }
        public bool IsHttps { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public Dictionary<string, string> Query { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public string Body { get; set; }
    }
}